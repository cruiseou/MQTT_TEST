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
        /// 一般警报
        /// </summary>
        private List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel> alarmsList = new List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel>();


        /// <summary>
        /// 断纤警报
        /// </summary>
        private List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel> faultlList = new List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel>();




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

            string filePath = System.Environment.CurrentDirectory;

            string filename = "AlarmAndFiberHistory-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls";
            if (!File.Exists(filePath + filename))
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.FillBackgroundColor = HSSFColor.Red.Index;
                ISheet AlarmSheet = workbook.CreateSheet("AlarmHistory ");
                IRow AlarmRow = AlarmSheet.CreateRow(0);
                string[] AlarmCellName = { "TypeID","TypeName","Level","Possibility","CenterPosition","EventWidth", "FirstPushTime", "LastPushTime",   "MaxIntensity","SensorID","ChannelID","PushTime"};
                for (int i = 0; i < AlarmCellName.Length; i++)
                {
                    ICell cell = AlarmRow.CreateCell(i, CellType.String);
                    cell.SetCellValue(AlarmCellName[i]);
                }
                ISheet FiberSheet = workbook.CreateSheet("FiberHistory ");
                IRow FiberRow = FiberSheet.CreateRow(0);
                string[] FibermCellName =
                {
                    "FiberStatus", "FiberBreakLength", "SensorID",
                    "ChannelID", "PushTime"
                };

                for (int i = 0; i < FibermCellName.Length; i++)
                {
                    ICell cell = FiberRow.CreateCell(i, CellType.String);
                    cell.SetCellValue(FibermCellName[i]);
                }
                FileStream fs = new FileStream(Path.Combine(filePath, filename), FileMode.Create);
                workbook.Write(fs);
                fs.Close();
                fs.Dispose();
            }
        }



        /// <summary>
        /// 警报信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttService_AlaramDataBing(object sender, DataBingArgs<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel> e)
        {

            if (alarmsList.Count >= 100)
            {
                alarmsList.RemoveAt(99);
            }
            alarmsList.AddRange(e.DataItems);

            dataGridView2.Invoke(new MethodInvoker(delegate
            {
                this.dataGridView2.DataSource = alarmsList.ToArray();
                this.dataGridView2.Refresh();
            }));

        }


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

            if (faultlList.Count >= 100)
            {
                faultlList.RemoveAt(99);
            }
            faultlList.AddRange(e.DataItems);

            dataGridView1.Invoke(new MethodInvoker(delegate
            {
                dataGridView1.DataSource = faultlList.ToArray();
                dataGridView1.Refresh();
            }));
        }
    }
}
