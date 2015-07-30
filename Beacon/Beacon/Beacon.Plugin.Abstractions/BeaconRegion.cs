using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beacon.Plugin.Abstractions
{
    public class BeaconRegion
    {
        public BeaconRegion(string identifier, string uuid, ushort major = ushort.MaxValue, ushort minor = 1,bool notifyOnEntry = true, bool notifyOnExit = true, bool notifyOnStay = false, bool showNotification = true)
        {
            this.UUID = uuid;
            this.Identifier = identifier;
            this.Major = major;
            this.Minor = minor;
            this.NotifyOnEntry = notifyOnEntry;
            this.NotifyOnExit = notifyOnExit;
            this.NotifyOnStay = notifyOnStay;
            this.ShowNotification = showNotification;
        }
        public string Identifier { get; private set; }
        public string UUID { get; private set; }
        public ushort Major { get; private set; }
        public ushort Minor { get; private set; }
        public bool NotifyOnEntry { get;  set; }
        public bool NotifyOnExit { get;  set; }
        public bool NotifyOnStay { get;  set; }
        public bool ShowNotification { get; set; }

        
        
    }
}
