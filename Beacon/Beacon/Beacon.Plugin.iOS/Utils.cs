using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Beacon.Plugin.Abstractions;
using MonoTouch.CoreLocation;

namespace Beacon.Plugin
{
    internal static class Utils
    {
        internal static CLBeaconRegion ToNative(this BeaconRegion region)
        {
            return new CLBeaconRegion(new NSUuid(region.UUID), region.Identifier) {
                NotifyOnEntry = true,
                NotifyOnExit = true,
                NotifyEntryStateOnDisplay = true
            };

        }

        internal static BeaconRegion ToBeaconRegion(this CLBeaconRegion region)
        {
            return new BeaconRegion(
                region.ProximityUuid.AsString(),
                region.Identifier,
                region.Major.UInt16Value,
                region.Minor.UInt16Value
            );
        }


        internal static BeaconItem ToBeaconItem(this CLBeaconRegion region, CLBeacon beacon)
        {
            return new BeaconItem(region.Identifier, region.ProximityUuid.AsString(), beacon.Rssi, region.Major.UInt16Value, region.Minor.UInt16Value, beacon.Proximity.ToBeaconProximity());
        }

        internal static BeaconProximity ToBeaconProximity(this CLProximity proximity)
        {
            switch (proximity)
            {
                case CLProximity.Far:
                    return BeaconProximity.Far;

                case CLProximity.Near:
                    return BeaconProximity.Near;

                case CLProximity.Immediate:
                    return BeaconProximity.Immediate;

                case CLProximity.Unknown:
                default:
                    return BeaconProximity.Unknown;
            }
        }
    }
}