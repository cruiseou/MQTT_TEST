using System;

namespace DFVSMQTTMessageDisplay.Model
{
    public class EventEntity
    {
        /// <summary>
        /// 通道号
        /// </summary> 
        public int ChannelId { set; get; }

        /// <summary>
        /// 故障类型
        /// </summary> 
        public EventType EventType { set; get; }

        /// <summary>
        /// 通道的长度  当光纤断裂时显示
        /// </summary>
        public float ChannelLength { set; get; }

        /// <summary>
        /// 发生时间
        /// </summary> 

        public DateTime TimeStamp { set; get; }
    }
}