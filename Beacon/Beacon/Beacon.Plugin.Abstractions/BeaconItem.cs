using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beacon.Plugin.Abstractions
{
    public class BeaconItem
    {
        public BeaconItem(string identifier,string uuid, int rssi,ushort major, ushort minor,BeaconProximity proximity)
        {
            Identifier = identifier;
            UUID = uuid;
            RSSI = rssi;
            Major = major;
            Minor = minor;
            Proximity = proximity;
        }

        public BeaconProximity Proximity { get; internal set; }
        public int RSSI { get; internal set; }
        public string UUID { get; internal set; }
        public string Identifier { get; internal set; }
        public ushort Major { get; internal set; }
        public ushort Minor { get; internal set; }

    }
}
