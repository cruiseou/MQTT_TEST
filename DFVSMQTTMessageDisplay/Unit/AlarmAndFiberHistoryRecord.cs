using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

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
            if (alarmsList.Count >= 10)
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
             
            }
        }



        /// <summary>
        /// 添加光纤状态警报信息
        /// </summary>
        /// <param name="fault"></param>

        public void AddFiberData(List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel> fault)
        {
            faultlList.AddRange(fault);
            if (faultlList.Count >= 10)
            {
                try
                {
                    List<DFVSChannelFiber> DFVSChannelAlarmlist = new List<DFVSChannelFiber>();
                    for (int i = 0; i < faultlList.Count; i++)
                    {
                        //if (faultlList[i].FiberStatus!=1|| faultlList[i].FiberStatus != 0)
                        //{
                            DFVSChannelAlarmlist.Add(new DFVSChannelFiber()
                            {
                                FiberStatus = faultlList[i].FiberStatus,
                                FiberBreakLength = faultlList[i].FiberBreakLength,
                                PushTime = faultlList[i].PushTime,
                                SensorID = faultlList[i].SensorID,
                                ChannelID = faultlList[i].ChannelID
                            });
                       // }

                       
                    }

                    if (DFVSChannelAlarmlist!=null && DFVSChannelAlarmlist.Count>0)
                    {
                        atenDBContainer ef = new atenDBContainer();
                        ef.DFVSChannelFibers.AddRange(DFVSChannelAlarmlist);
                        ef.SaveChanges();

                    }
                  

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                faultlList.Clear();
            }

        }


        /// <summary>
        /// 查询光纤状态信息
        /// </summary>
        public void QueryChannelFiberHistory(int PageIndex, int PageSize)
        {
            int count = 0;
            atenDBContainer ef = new atenDBContainer();
            Expression<Func<DFVSChannelFiber, bool>> wh = c => true;

            if (!string.IsNullOrEmpty(dateTimePicker3.ToString()) && !string.IsNullOrEmpty(dateTimePicker4.ToString()))
            {

                DateTime start = dateTimePicker3.Value;
                DateTime end = dateTimePicker4.Value.AddDays(1);
                wh = wh.And(c => c.PushTime > start && c.PushTime < end);
            }
            var q = from m1 in ef.DFVSChannelFibers.Where(wh).OrderBy(o=>o.PushTime)  .Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList()
                select new DFVSChannelFiber
                {
                    ID = m1.ID,
                    FiberStatus = m1.FiberStatus,
                    FiberBreakLength = m1.FiberBreakLength,
                    SensorID = m1.SensorID,
                    ChannelID = m1.ChannelID,
                    PushTime = m1.PushTime,

                };
            count = ef.DFVSChannelFibers.Where(wh).Count();
           
            pagerControl1.DrawControl(count);
            dataGridView1.Invoke(new MethodInvoker(delegate
            {
              
                dataGridView1.DataSource = q.ToArray();
                dataGridView1.Refresh();
            }));

        }


        /// <summary>
        /// 查询警报信息
        /// </summary>

        public void QueryChannelAlarmHistory(int PageIndex, int PageSize)
        {
            int count = 0;
            atenDBContainer ef = new atenDBContainer();
            Expression<Func<DFVSChannelAlarm, bool>> wh = c => true;

            if (!string.IsNullOrEmpty(dateTimePicker1.ToString()) && !string.IsNullOrEmpty(dateTimePicker2.ToString()))
            {
                DateTime start = dateTimePicker1.Value;
                DateTime end = dateTimePicker2.Value.AddDays(1);
                wh = wh.And(c => c.PushTime > start && c.PushTime < end);
            }

            if (comboBox1.SelectedItem!=null)
            {

                int alarmLevel = Convert.ToInt32(comboBox1.SelectedItem);
                wh = wh.And(c => c.AlarmLevel == alarmLevel);

            }
            var q = from m1 in ef.DFVSChannelAlarms.Where(wh).OrderBy(o => o.PushTime).Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList()
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

                    };
            count = ef.DFVSChannelAlarms.Where(wh).Count();
           

            pagerControl2.DrawControl(count);
            dataGridView2.Invoke(new MethodInvoker(delegate
            {
               
                dataGridView2.DataSource = q.ToArray();
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
            dateTimePicker3.Value = DateTime.Now.AddDays(-7);
            dateTimePicker1.Value = DateTime.Now.AddDays(-7);

            pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);

            pagerControl2.OnPageChanged+=new EventHandler(pagerControl2_OnPageChanged);

         

        }

        void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            QueryChannelFiberHistory(pagerControl1.PageIndex, pagerControl1.PageSize);
        }

        void pagerControl2_OnPageChanged(object sender, EventArgs e)
        {
            QueryChannelAlarmHistory(pagerControl2.PageIndex, pagerControl2.PageSize);
        }

        /// <summary>
        /// 断纤状态历史警报查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            QueryChannelFiberHistory(pagerControl1.PageIndex,pagerControl1.PageSize);
        }


        /// <summary>
        /// 警报状态历史记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            QueryChannelAlarmHistory(pagerControl2.PageIndex, pagerControl2.PageSize);
        }
    }
}
