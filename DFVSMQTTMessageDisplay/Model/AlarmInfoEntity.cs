using Newtonsoft.Json;
using System;

namespace DFVSMQTTMessageDisplay.Model
{
    public class AlarmInfoEntity
    {
        
        
        /// <summary>
        /// 设备唯一标识（设备名）
        /// </summary>
      
        public string device_id { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        
        public int channel_id { get; set; }

        /// <summary>
        /// 报警类型
        /// </summary>
      
        public int alarm_id { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        
        public int level { get; set; }

        /// <summary>
        /// 报警类型
        /// </summary>
       
        public string alarm_type { get; set; }

        /// <summary>
        /// 报警类型可能性
        /// </summary>
    
        public float possibility { get; set; }

        /// <summary>
        /// 事件中心点位置
        /// </summary>
     
        public float center { get; set; }

        /// <summary>
        /// 事件宽度
        /// </summary>
      
        public float width { get; set; }

        /// <summary>
        /// 事件创建时间（初始时间）
        /// </summary>
       
        public DateTime create_date { get; set; }

        /// <summary>
        /// 最后上报时标（事件最后刷新时间）
        /// </summary>
       
        public DateTime timestamp { get; set; }

        /// <summary>
        /// 最大强度
        /// </summary>
        
        public float max_intensity { get; set; }
    }
}