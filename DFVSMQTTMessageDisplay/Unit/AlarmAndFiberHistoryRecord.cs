using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ATIAN.Common.MQTTLib.Protocol.DFVS;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Reflection;
using System.Data.OleDb;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;

namespace DFVSMQTTMessageDisplay.Unit
{
    public partial class AlarmAndFiberHistoryRecord : UserControl
    {    /// <summary>
         /// 一般警报
         /// </summary>
        private List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel> alarmsList = new List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel>();

        /// <summary>
        /// 断纤警报
        /// </summary>
        private List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel> faultlList = new List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel>();


        public AlarmAndFiberHistoryRecord()
        {
            InitializeComponent();

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
        /// 添加警报信息
        /// </summary>
        /// <param name="alarms"></param>
        public void AddAlarmData(List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel> alarms)
        {
            alarmsList.AddRange(alarms);
            if (alarmsList.Count >= 100)
            {
                try
                {
                    List<DFVSChannelAlarm> DFVSChannelAlarmlist = new List<DFVSChannelAlarm>();
                    for (int i = 0; i < alarmsList.Count; i++)
                    {
                        DFVSChannelAlarmlist.Add(new DFVSChannelAlarm()
                        {
                            TypeID = alarmsList[i].TypeID,
                            TypeName = alarmsList[i].TypeName,
                            AlarmLevel = alarmsList[i].Level,
                            Possibility = alarmsList[i].Possibility,
                            CenterPosition = alarmsList[i].CenterPosition,
                            EventWidth = alarmsList[i].EventWidth,
                            FirstPushTime = alarmsList[i].FirstPushTime,
                            LastPushTime = alarmsList[i].LastPushTime,
                            PushTime = alarmsList[i].PushTime,
                            MaxIntensity = alarmsList[i].MaxIntensity,
                            SensorID = alarmsList[i].SensorID,
                            ChannelID = alarmsList[i].ChannelID
                        });

                    }
                    atenDBContainer ef = new atenDBContainer();
                    ef.DFVSChannelAlarms.AddRange(DFVSChannelAlarmlist);
                    ef.SaveChanges();
                    alarmsList.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                alarmsList.Clear();
                QueryChannelAlarmHistory();
            }
        }



        /// <summary>
        /// 添加光纤状态警报信息
        /// </summary>
        /// <param name="fault"></param>

        public void AddFiberData(List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel> fault)
        {
            faultlList.AddRange(fault);
            if (faultlList.Count >= 100)
            {
                try
                {
                    List<DFVSChannelFiber> DFVSChannelAlarmlist = new List<DFVSChannelFiber>();
                    for (int i = 0; i < faultlList.Count; i++)
                    {
                        DFVSChannelAlarmlist.Add(new DFVSChannelFiber()
                        {
                            FiberStatus = faultlList[i].FiberStatus,
                            FiberBreakLength = faultlList[i].FiberBreakLength,
                            PushTime = faultlList[i].PushTime,
                            SensorID = faultlList[i].SensorID,
                            ChannelID = faultlList[i].ChannelID
                        });
                    }
                    atenDBContainer ef = new atenDBContainer();
                    ef.DFVSChannelFibers.AddRange(DFVSChannelAlarmlist);
                    ef.SaveChanges();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                QueryChannelFiberHistory();
                faultlList.Clear();
            }

        }


        /// <summary>
        /// 查询光纤状态信息
        /// </summary>
        public void QueryChannelFiberHistory()
        {
            atenDBContainer ef = new atenDBContainer();
            IList<DFVSChannelFiber> ChannelFiberList = ef.DFVSChannelFibers.Where(o => o.ID != 0).ToList();

            dataGridView1.Invoke(new MethodInvoker(delegate
            {
                dataGridView1.DataSource = ChannelFiberList.ToArray();
                dataGridView1.Refresh();
            }));

        }


        /// <summary>
        /// 查询警报信息
        /// </summary>

        public void QueryChannelAlarmHistory()
        {
            atenDBContainer ef = new atenDBContainer();
            IList<DFVSChannelAlarm> ChannelAlarmList = ef.DFVSChannelAlarms.Where(o => o.ID != 0).ToList();
            dataGridView2.Invoke(new MethodInvoker(delegate
            {
                dataGridView2.DataSource = ChannelAlarmList.ToArray();
                dataGridView2.Refresh();
            }));


        }

        /// <summary>
        /// 窗体加载的时候查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmAndFiberHistoryRecord_Load(object sender, EventArgs e)
        {
            QueryChannelAlarmHistory();

            QueryChannelFiberHistory();
        }


        /// <summary>
        /// 断纤状态历史警报查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            atenDBContainer ef = new atenDBContainer();
            Expression<Func<DFVSChannelFiber, bool>> wh = c => true;

            if (!string.IsNullOrEmpty(dateTimePicker3.ToString()) && !string.IsNullOrEmpty(dateTimePicker4.ToString()))
            {

                wh = wh.And(c => c.PushTime > dateTimePicker3.Value && c.PushTime < dateTimePicker4.Value);
            }
            var q = from m1 in ef.DFVSChannelFibers.Where(wh).ToList()
                    select new DFVSChannelFiber
                    {
                        ID = m1.ID,
                        FiberStatus = m1.FiberStatus,
                        FiberBreakLength = m1.FiberBreakLength,
                        SensorID = m1.SensorID,
                        ChannelID = m1.ChannelID,
                        PushTime = m1.PushTime,

                    };
            dataGridView1.Invoke(new MethodInvoker(delegate
            {
                dataGridView1.DataSource = q.ToArray();
                dataGridView1.Refresh();
            }));
        }


        /// <summary>
        /// 警报状态历史记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            atenDBContainer ef = new atenDBContainer();
            Expression<Func<DFVSChannelAlarm, bool>> wh = c => true;

            if (!string.IsNullOrEmpty(dateTimePicker1.ToString()) && !string.IsNullOrEmpty(dateTimePicker2.ToString()))
            {

                wh = wh.And(c => c.PushTime > dateTimePicker1.Value && c.PushTime < dateTimePicker2.Value);
            }
            if (comboBox1.SelectedItem!=null)
            {
                int AlarmLevel = Convert.ToInt32(comboBox1.SelectedItem);

                wh = wh.And(c => c.AlarmLevel==AlarmLevel);
            }
            var q = from m1 in ef.DFVSChannelAlarms.Where(wh).ToList()
                    select new DFVSChannelAlarm
                    {
                        ID = m1.ID,
                        TypeID = m1.TypeID,
                        TypeName = m1.TypeName,
                        AlarmLevel = m1.AlarmLevel,
                        Possibility = m1.Possibility,
                        CenterPosition = m1.CenterPosition,
                        EventWidth = m1.EventWidth,
                        FirstPushTime = m1.FirstPushTime,
                        LastPushTime = m1.LastPushTime,
                        PushTime = m1.PushTime,
                        MaxIntensity = m1.MaxIntensity,
                        SensorID = m1.SensorID,
                        ChannelID = m1.ChannelID,

                    } ;
            dataGridView2.Invoke(new MethodInvoker(delegate
            {

                dataGridView2.DataSource = q.ToArray();
                dataGridView2.Refresh();
            }));
        }
    }
}
