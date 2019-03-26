using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFVSMQTTMessageDisplay.Unit
{
    public partial class AlarmAndFiberRealTimeRecord : UserControl
    {

        /// <summary>
        /// 一般警报
        /// </summary>
        private List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel> alarmsList = new List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel>();

        /// <summary>
        /// 断纤警报
        /// </summary>
        private List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel> faultlList = new List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel>();


        public AlarmAndFiberRealTimeRecord()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 显示自动增长行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView1.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);

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
        /// 清除警报
        /// </summary>
        public void ClearData()
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
        /// 添加警报信息
        /// </summary>
        /// <param name="alarms"></param>
        public void AddAlarmData(List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel> alarms)
        {
            if (alarmsList.Count >= 100)
            {
                alarmsList.RemoveAt(99);
            }
            alarmsList.AddRange(alarms);

            dataGridView2.Invoke(new MethodInvoker(delegate
            {
                this.dataGridView2.DataSource = alarmsList.ToArray();
                this.dataGridView2.Refresh();
            }));
        }



        /// <summary>
        /// 添加光纤状态警报信息
        /// </summary>
        /// <param name="fault"></param>

        public void AddFiberData(List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel> fault)
        {
            if (faultlList.Count >= 100)
            {
                faultlList.RemoveAt(99);
            }
            faultlList.AddRange(fault);

            dataGridView1.Invoke(new MethodInvoker(delegate
            {
                dataGridView1.DataSource = faultlList.ToArray();
                dataGridView1.Refresh();
            }));
        }

    }
}
