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
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 清除实时警报数据
        /// </summary>
        public void DataClear()
        {
            alarmAndFiberRealTimeRecord1.ClearData();
        }


        /// <summary>
        /// 添加警报
        /// </summary>
        /// <param name="alarmsList"></param>
        public void AddAlarmsRecord(List<ATIAN.Common.MQTTLib.Protocol.DFVS.ChannelAlarmModel> alarmsList)
        {
            alarmAndFiberRealTimeRecord1.AddAlarmData(alarmsList);
            alarmHistory1.AddAlarmData(alarmsList);
        }


        /// <summary>
        /// 添加光纤状态警报
        /// </summary>
        /// <param name="faultlList"></param>

        public   void AddFiberRecord(List<ATIAN.Common.MQTTLib.Protocol.ChannelFiberModel> faultlList)
        {
            faultlList = faultlList.Where(o => o.FiberStatus != 7).ToList();
            alarmAndFiberRealTimeRecord1.AddFiberData(faultlList);

           
            alarmHistory1.AddFiberData(faultlList);
        }



    }
}
