using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Data;
using System.Drawing;
using System.Linq;
using DFVSMQTTMessageDisplay.Model;
using MQTTnet.Protocol;

namespace DFVSMQTTMessageDisplay
{
    public partial class MQTTMessageDisplay : Form
    {


        /// <summary>
        /// 一般警报
        /// </summary>
        private List<AlarmInfoEntity> alarmsList = new List<AlarmInfoEntity>();


        /// <summary>
        /// 断纤警报
        /// </summary>
        private List<EventEntity> faultlList = new List<EventEntity>();

        /// <summary>
        /// mqtt客户端
        /// </summary>
        private IManagedMqttClient mqttClient;

        private ManagedMqttClientOptionsBuilder managedMqttClientOptionsBuilder;

        private MqttClientOptionsBuilder mqrrOptionsBuilder;

        /// <summary>
        /// 
        /// </summary>
        private String clientID = Guid.NewGuid().ToString();


        public MQTTMessageDisplay()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        /// <summary>
        /// 窗体初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            mqttClient = new MqttFactory().CreateManagedMqttClient();
            //链接事件
            mqttClient.Connected += MqttClient_Connected;
            //断开链接事件
            mqttClient.Disconnected += MqttClient_Disconnected;
            //连接失败事件
            mqttClient.ConnectingFailed += MqttClient_ConnectingFailed;
            //订阅失败事件
            mqttClient.SynchronizingSubscriptionsFailed += MqttClient_SynchronizingSubscriptionsFailed;
            //数据接收事件
            mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
           
        }
        /// <summary>
        /// 启动mqtt监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MQTTStar_Click(object sender, EventArgs e)
        {
            if (mqttClient.IsStarted)
            {
                mqttClient.StopAsync();
                btn_MQTTStar.Invoke(new MethodInvoker(delegate
                {
                    btn_MQTTStar.Text = "启动MQTT监听";
                }));

               
            }
            else
            {
                mqrrOptionsBuilder = new MqttClientOptionsBuilder();
                mqrrOptionsBuilder.WithClientId(clientID);
                mqrrOptionsBuilder.WithCleanSession(true);
                mqrrOptionsBuilder.WithTcpServer(txt_IP.Text.Trim(), 1883);
                managedMqttClientOptionsBuilder = new ManagedMqttClientOptionsBuilder();
                managedMqttClientOptionsBuilder.WithAutoReconnectDelay(TimeSpan.FromSeconds(10));
             
                managedMqttClientOptionsBuilder.WithClientOptions(mqrrOptionsBuilder);
                mqttClient.StartAsync(managedMqttClientOptionsBuilder.Build());
                btn_MQTTStar.Invoke(new MethodInvoker(delegate
                {
                    btn_MQTTStar.Text = "停止MQTT监听";
                }));

               
            }


        }

        /// <summary>
        /// 连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_Connected(object sender, MqttClientConnectedEventArgs e)
        {

        }

        /// <summary>
        /// 断开链接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_Disconnected(object sender, MqttClientDisconnectedEventArgs e)
        {

        }

        /// <summary>
        /// 连接失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_ConnectingFailed(object sender, MqttManagedProcessFailedEventArgs e)
        {

        }

        /// <summary>
        /// 订阅失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_SynchronizingSubscriptionsFailed(object sender, MqttManagedProcessFailedEventArgs e)
        {

        }

        /// <summary>
        /// mqtt消息接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            byte[] buffPayLoad = e.ApplicationMessage.Payload;
            var payloadString = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            //一般警报
            if (e.ApplicationMessage.Topic.Equals("DFVS/Alarms"))
            {
                var model = JsonConvert.DeserializeObject<AlarmInfoEntity>(payloadString);

                if (alarmsList.Count >= 100)
                {
                    alarmsList.RemoveAt(99);
                }
                alarmsList.Insert(0, model);

                dataGridView2.Invoke(new MethodInvoker(delegate
                {
                    this.dataGridView2.DataSource = alarmsList.ToArray();
                    this.dataGridView2.Refresh();
                }));
            }
            //断纤警报
            else if (e.ApplicationMessage.Topic.Equals("DFVS/FiberStatus"))
            {
                var model = JsonConvert.DeserializeObject<EventEntity>(payloadString);
                if (faultlList.Count >= 100)
                {
                    faultlList.RemoveAt(99);
                }
                faultlList.Insert(0, model);

                dataGridView1.Invoke(new MethodInvoker(delegate
                {
                    dataGridView1.DataSource = faultlList.ToArray();
                    dataGridView1.Refresh();
                }));
            }
        }

        /// <summary>
        /// 监听断纤警报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Broken_Click(object sender, EventArgs e)
        {
            try
            {
                mqttClient.SubscribeAsync("DFVS/FiberStatus");
            }
            catch (Exception exception)
            {

            }

        }

        /// <summary>
        ///  监听一般警报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Alarm_Click(object sender, EventArgs e)
        {
            try
            {
                mqttClient.SubscribeAsync("DFVS/Alarms");
            }
            catch (Exception exception)
            {

            }


        }

        /// <summary>
        /// 显示自动增长行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView2.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView2.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);

        }

        /// <summary>
        ///  显示自动增长行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView1.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);

        }

        /// <summary>
        /// 清除警报信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            //断纤警报清除
            dataGridView1.Invoke(new MethodInvoker(delegate
            {
                faultlList.Clear();
                dataGridView1.DataSource = faultlList.ToArray();
                dataGridView1.Refresh();
            }));

            //一般警报清除
            dataGridView2.Invoke(new MethodInvoker(delegate
            {
                alarmsList.Clear();
                dataGridView2.DataSource = alarmsList.ToArray();
                dataGridView2.Refresh();
            }));

        }


        /// <summary>
        /// 吸合继电器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_open_Click(object sender, EventArgs e)
        {
            try
            {
                if (!mqttClient.IsConnected)
                {
                    MessageBox.Show("MQTT服务未连接","警告",MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation);
                    return;
                }
                this.mqttClient.PublishAsync(new ManagedMqttApplicationMessage()
                {
                    ApplicationMessage = new MqttApplicationMessage()
                    {
                        Payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Closures = new int[] { 1, 2, 3, 4, 5, 6, 7,8 } })),
                        QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
                        Topic = "Relay/00000000-0000-0000-0000-000000000000/Control"
                    }
                });
            }
            catch (Exception exception)
            {

            }
        }

        /// <summary>
        /// 断开继电器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, EventArgs e)
        {

           
            try
            {
                if (!mqttClient.IsConnected)
                {
                    MessageBox.Show("MQTT服务未连接", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    return;
                }


                this.mqttClient.PublishAsync(new ManagedMqttApplicationMessage()
                {

                
                    ApplicationMessage = new MqttApplicationMessage()
                    {
                        Payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Ups = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 } })),
                        QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
                        Topic = "Relay/00000000-0000-0000-0000-000000000000/Control"
                    }
                });
            }
            catch (Exception exception)
            {

            }
        }

    }
}
