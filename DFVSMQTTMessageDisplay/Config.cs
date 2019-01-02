using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFVSMQTTMessageDisplay
{

    internal class LocalSetting
    {
        public string Alarms { get; set; }
        public string Hostname { get; set; }
        public string Fault { get; set; }
        public string Binarization { get; set; }
    }

    internal class RemoteSetting
    {
        public string ConnectionStatus { get; set; }
        public string Hostname { get; set; }
        public string Relay { get; set; }
        public string Threshold { get; set; }
        public string DFVSAlarms { get; set; }
    }



    internal class ApiSetting
    {
        public SensorSetting Sensor { get; set; }

        public UriSetting Uri { get; set; }
    }

    internal class SensorSetting
    {
        public string Name { get; set; }
    }

    internal class UriSetting
    {
        public string BaseUri { get; set; }
        public string AddDFVSAlarms { get; set; }
        public string IsAlarm { get; set; }
        public string IsBroken { get; set; }
        public string RelayStatus { get; set; }
        public string Threshold { get; set; }
        public string Sensor { get; set; }
        public string Heartbeat { get; set; }
    }
    internal class RelaySetting
    {
        public RelayAlarmSetting[] Alarm { get; set; }
        public int[] Broken { get; set; }
    }
    internal class RelayAlarmSetting
    {
        public int Begin { get; set; }
        public int End { get; set; }
        public int[] Relays { get; set; }
        public int MinLevel { get; set; }
    }
    internal class LanguageSetting
    {
        public string AlarmTopic { get; set; }
        public string Alarms { get; set; }
        public string BrokenTopic { get; set; }
        public string Broken { get; set; }
    }

    internal class Config
    {
        public LocalSetting Local { get; set; }
        public SensorSetting Sensor { get; set; }
        public bool IsEnableRemote { get; set; }
        public RemoteSetting Remote { get; set; }
        public ApiSetting Api { get; set; }
        public bool IsEnableRelay { get; set; }
        public RelaySetting Relays { get; set; }
        public LanguageSetting Language { get; set; }
    }



}
