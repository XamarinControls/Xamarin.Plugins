using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beacon.Plugin.Abstractions
{
    public interface IBeaconListener
    {
        void OnMonitoringStarted(string identifier);
        void OnRangingStarted(string identifier);
        void OnMonitoringStopped();
        void OnMonitoringStopped(string identifier);
        void OnRangingStopped();
        void OnRangingStopped(string identifier);
        void OnAdvertisingStarted();
        void OnAdvertisingStopped();
        void OnRegionStateChanged();
        void OnRangingChange(BeaconItem item);
        void OnError(BeaconError error);
    }
}
