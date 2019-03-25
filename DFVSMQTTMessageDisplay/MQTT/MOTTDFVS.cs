
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATIAN.Common.ManagedInvoke;
using ATIAN.Common.MQTTLib.Protocol;
using ATIAN.Common.MQTTLib.Protocol.DFVS;
using ATIAN.Middleware.DFSensor;
using ATIAN.Middleware.DFVSensor;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace DFVSMQTTMessageDisplay.MQTT
{
    public class DataBingArgs<T> : EventArgs
    {
        public List<T> DataItems { get; set; }
    }

    public class MOTTDFVS : ManagedDFVSensor
    {
        public event EventHandler<DataBingArgs<ChannelAlarmModel>> AlaramDataBing;

        public event EventHandler<DataBingArgs<ChannelFiberModel>> FiberDataBing;


        public MOTTDFVS()
        {
            IsSubscribeTopicDFVSChannelFiber = true;
            IsSubscribeTopicDFVSChannelAlarm = true;
        }
        protected override void OnExecuteChannelAlarmStorage(ChannelAlarmModel channelAlarmModel)
        {
            List<ChannelAlarmModel> channelAlarmModels = new List<ChannelAlarmModel>();
            channelAlarmModels.Add(channelAlarmModel);
            AlaramDataBing?.Invoke(this, new DataBingArgs<ChannelAlarmModel>()
            {
                DataItems = channelAlarmModels
            });
           
        }

        protected override void OnExecuteChannelFiberStorage(ChannelFiberModel fiberStatusModel)


        {
            List<ChannelFiberModel> channelAlarmModels = new List<ChannelFiberModel>();
            channelAlarmModels.Add(fiberStatusModel);
            FiberDataBing?.Invoke(this, new DataBingArgs<ChannelFiberModel>()
            {
                DataItems = channelAlarmModels
            });




        }

        protected override void OnStarted()
        {

        }

        public void RelayControl(bool isOpen)
        {
            if (isOpen)
            {
                this.ManagedMQTTService.ManagedMqttClient.PublishAsync(new ManagedMqttApplicationMessage()
                {
                    ApplicationMessage = new MqttApplicationMessage()
                    {
                        Payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Closures = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 } })),
                        QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
                        Topic = "Relay/00000000-0000-0000-0000-000000000000/Control"
                    }
                });
            }
            else
            {
                this.ManagedMQTTService.ManagedMqttClient.PublishAsync(new ManagedMqttApplicationMessage()
                {
                    ApplicationMessage = new MqttApplicationMessage()
                    {
                        Payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Ups = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 } })),
                        QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
                        Topic = "Relay/00000000-0000-0000-0000-000000000000/Control"
                    }
                });

            }

        }



    }
}
