using System;
using System.Collections.Generic;

namespace Beacon.Plugin.Abstractions
{
  /// <summary>
  /// Interface for Beacon
  /// </summary>
  public interface IBeacon
  {

      IReadOnlyList<BeaconRegion> RangingRegions { get; }
      IReadOnlyList<BeaconRegion> MonitoringRegions { get; }
      /// <summary>
      /// Indicator that is true if at least one region is been monitored
      /// </summary>
      bool IsMonitoring { get; }
      bool IsAdvertising { get; }
      bool IsRanging { get; }
      //
      /// <summary>
      /// Starts monitoring one region
      /// </summary>
      /// <param name="regions"></param>
      void StartMonitoring(BeaconRegion region);
      /// <summary>
      /// Starts monitoring multiple regions
      /// </summary>
      /// <param name="regions"></param>
      void StartMonitoring(IList<BeaconRegion> regions);
      /// <summary>
      /// Stops monitoring one region
      /// </summary>
      /// <param name="identifier"></param>
      void StopMonitoring(BeaconRegion region);
      /// <summary>
      /// Stops monitoring multiple regions
      /// </summary>
      /// <param name="identifier"></param>
      void StopMonitoring(IList<BeaconRegion> regions);
      /// <summary>
      /// Stops monitoring all regions
      /// </summary>
      void StopMonitoringAllRegions();
      /// <summary>
      /// Starts ranging one region
      /// </summary>
      /// <param name="regions"></param>
      void StartRanging(BeaconRegion region);
      /// <summary>
      /// Starts ranging multiple regions
      /// </summary>
      /// <param name="regions"></param>
      void StartRanging(IList<BeaconRegion> regions);
      /// <summary>
      /// Stops ranging one region
      /// </summary>
      /// <param name="identifier"></param>
      void StopRanging(BeaconRegion region);
      /// <summary>
      /// Stops ranging multiple regions
      /// </summary>
      /// <param name="identifier"></param>
      void StopRanging(IList<BeaconRegion> regions);
      /// <summary>
      /// Stops ranging all regions
      /// </summary>
      void StopRangingAllRegions();
      //
      /// <summary>
      /// Starts advertising one region
      /// </summary>
      /// <param name="regions"></param>
      void StartAdvertising(BeaconRegion region, BeaconAdvertismentPowerLevel powerLevel);
     
      /// <summary>
      /// Stops advertising all regions
      /// </summary>
      void StopAdvertising();
    
     
     
  }
}
