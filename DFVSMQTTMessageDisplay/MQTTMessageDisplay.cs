using DFVSMQTTMessageDisplay.Model;
using DFVSMQTTMessageDisplay.MQTT;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DFVSMQTTMessageDisplay
{
    public partial class MQTTMessageDisplay : Form
    {
        private MOTTDFVS mqttService = null;

        /// <summary>
        /// 
        /// </summary>
        private String clientID = Guid.NewGuid().ToString();

        /// <summary>
        /// 是否自动关闭计时器
        /// </summary>
        private bool ischecked = false;

        //定义Timer类变量
        private System.Timers.Timer Mytimer;
        long TimeCount;

     
        public MQTTMessageDisplay()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 警报信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttService_AlaramDataBing(object sender, DataBingArgs<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel> e)
        {
            e.DataItems = e.DataItems.Where(o => o.Level > 0).ToList();
            userControl11.AddAlarmsRecord(e.DataItems);
        }

        /// <summary>
        /// 定时关闭继电器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mytimer_tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (checkBox1.Checked)
            {
                try
                {
                    mqttService.RelayControl(false);
                }
                catch (Exception exception)
                {

                }

            }
        }

        /// <summary>
        /// 清除警报信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            userControl11.DataClear();
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
                mqttService.RelayControl(true);
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
            mqttService.RelayControl(false);
        }

        /// <summary>
        /// 限制只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键  
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字  
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 是否启动延时关闭继电器功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("延时关闭时间不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Invoke(new MethodInvoker(delegate { checkBox1.Checked = false; }));
                    return;
                }
                this.Invoke(new MethodInvoker(delegate { textBox1.Enabled = false; }));

                Mytimer = new System.Timers.Timer(Convert.ToInt32(textBox1.Text.Trim()) * 60 * 1000);
                //设置重复计时
                Mytimer.AutoReset = true;
                //设置执行System.Timers.Timer.Elapsed事件

                Mytimer.Elapsed += new System.Timers.ElapsedEventHandler(Mytimer_tick);
                Mytimer.Start();
            }
            else
            {
                if (Mytimer != null)
                {
                    Mytimer.Stop();
                    this.Invoke(new MethodInvoker(delegate { textBox1.Enabled = true; }));
                }
            }
        }

        /// <summary>
        /// 窗体加载完成后启动mqtt服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MQTTMessageDisplay_Load(object sender, EventArgs e)
        {
            mqttService = new MOTTDFVS();
            mqttService.AlaramDataBing += MqttService_AlaramDataBing;
            mqttService.FiberDataBing += MqttService_FiberDataBing;
            mqttService.Start("127.0.0.1", 1883);

        }

        /// <summary>
        /// 光纤状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttService_FiberDataBing(object sender, DataBingArgs<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel> e)
        {

            e.DataItems = e.DataItems.Where(o => o.FiberStatus !=11 && o.FiberStatus!=7).ToList();
            userControl11.AddFiberRecord(e.DataItems);

        }
    }
}
