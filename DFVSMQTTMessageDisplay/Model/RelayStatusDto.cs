using System;

namespace DFVSMQTTMessageDisplay.Model
{
    public class RelayStatusDto
    {
        public Guid DeviceID { get; set; }
        public bool RelayStatus { get; set; }
        public string ModifiedBy { get; set; }
    }
}