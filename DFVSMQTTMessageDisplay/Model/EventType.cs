using System;

namespace DFVSMQTTMessageDisplay.Model
{
    
    public enum EventType
    {

        /// <summary>
        /// 光纤正常
        /// </summary>
        Ok = 1,

        /// <summary>
        /// 光纤插入
        /// </summary>
        Fin = 2,



        /// <summary>
        /// 光纤过长
        /// </summary>
        ToLong = 4,

        /// <summary>
        /// 光纤断裂
        /// </summary>
        Break = 8,

        /// <summary>
        /// 光纤损耗过多
        /// </summary>
        ToLoss = 16,


        /// <summary>
        /// 拔出
        /// </summary>
        Pullout =0,


        /// <summary>
        /// 插入
        /// </summary>
        Insert = 11,


        /// <summary>
        /// 拔出
        /// </summary>
        OUTOUT=7,

        /// <summary>
        /// 未知
        /// </summary>
        Unknown=3

    }
}