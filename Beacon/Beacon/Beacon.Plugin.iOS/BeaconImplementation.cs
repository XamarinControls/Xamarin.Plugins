using Beacon.Plugin.Abstractions;
#if __UNIFIED__
 using CoreBluetooth;
 using CoreFoundation;
 using CoreLocation;
 using Foundation;
 using UIKit;
#else
 using MonoTouch.CoreBluetooth;
 using MonoTouch.CoreFoundation;
 using MonoTouch.CoreLocation;
 using MonoTouch.Foundation;
 using MonoTouch.UIKit;
#endif

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Beacon.Plugin
{
  /// <summary>
  /// Implementation for Beacon
  /// </summary>
  public class BeaconImplementation : IBeacon
  {
      CBPeripheralManager peripheralManager;
      BTPeripheralDelegate peripheralDelegate;
      CLLocationManager locationManager;
      CLProximity previousProximity;
      
      private Dictionary<string, BeaconRegion> mRegions;
      /// <summary>
      /// Monitored regions
      /// </summary>
      public IReadOnlyDictionary<string, BeaconRegion> Regions { get { return mRegions; } }

      public BeaconImplementation()
      {
          mRegions =new Dictionary<string, BeaconRegion>();

          locationManager = new CLLocationManager();
          //locationManager.DidStartMonitoringForRegion += DidStartMonitoringForRegion;
          locationManager.RegionEntered += RegionEntered;
          locationManager.RegionLeft +=RegionLeft;
          locationManager.DidRangeBeacons+=DidRangeBeacons;


          

         
      }

      /// <summary>
      /// Starts monitoring region
      /// </summary>
      /// <param name="region"></param>
      public void StartMonitoring(BeaconRegion region)
      {

          AddRegion(region);
      }

      void AddRegion(BeaconRegion region)
      {
          CLRegion cRegion = null;


          if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
          {
              CLBeaconRegion beaconRegion= new CLBeaconRegion(new NSUuid(region.UUID),region.Major,region.Minor,region.Identifier);
              beaconRegion.NotifyEntryStateOnDisplay = true;
              cRegion = beaconRegion;
          }
    

          cRegion.NotifyOnEntry = region.NotifyOnEntry || region.NotifyOnStay;
          cRegion.NotifyOnExit = region.NotifyOnExit;
         


          locationManager.StartMonitoring(cRegion);
          locationManager.RequestState(cRegion);
      }

      void DidRangeBeacons(object sender, CLRegionBeaconsRangedEventArgs e)
      {
          if (e.Beacons.Length > 0)
          {
              
              CLBeacon beacon = e.Beacons[0];
              //string message = "";
              BeaconItem item = new BeaconItem(e.Region.Identifier, beacon.ProximityUuid.AsString(), (int)beacon.Rssi, beacon.Major.UInt16Value, beacon.Minor.UInt16Value, beacon.Proximity.ToBeaconProximity());
              CrossBeacon.BeaconListener.OnRangingChange(item);


              if (previousProximity != beacon.Proximity)
              {
                 // CrossBeacon.BeaconListener.OnRangingChange(item);
  
              }
              previousProximity = beacon.Proximity;
          }
 	    
      }


      void RegionEntered(object sender, CLRegionEventArgs e)
      {
          
          OnRegionEntered(e.Region);
        
      }
      void OnRegionEntered(CLRegion region)
      {
          locationManager.StartRangingBeacons((CLBeaconRegion)region);
      }

      void RegionLeft(object sender, CLRegionEventArgs e)
      {
          
          OnRegionLeft(e.Region);
        
      }
      void OnRegionLeft(CLRegion region)
      {  
          locationManager.StopRangingBeacons((CLBeaconRegion)region);
      }

      void CreateNotification(string title, string message)
      {
          UILocalNotification notification = new UILocalNotification();

          notification.AlertAction = title;
          notification.AlertBody = message;
          notification.HasAction = true;

          notification.SoundName = UILocalNotification.DefaultSoundName;
          #if __UNIFIED__
            UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
          #else
            UIApplication.SharedApplication.PresentLocationNotificationNow(notification);
          #endif

      }

       

      public IReadOnlyList<BeaconRegion> RangingRegions
      {
          get { throw new NotImplementedException(); }
      }

      public IReadOnlyList<BeaconRegion> MonitoringRegions
      {
          get { throw new NotImplementedException(); }
      }

      public void StartMonitoring(IList<BeaconRegion> regions)
      {
          throw new NotImplementedException();
      }

      public void StopMonitoring(BeaconRegion region)
      {
          throw new NotImplementedException();
      }

      public void StopMonitoring(IList<BeaconRegion> regions)
      {
          throw new NotImplementedException();
      }

      public void StopMonitoringAllRegions()
      {
          throw new NotImplementedException();
      }

      public void StartRanging(BeaconRegion region)
      {
          throw new NotImplementedException();
      }

      public void StartRanging(IList<BeaconRegion> regions)
      {
          throw new NotImplementedException();
      }

      public void StopRanging(BeaconRegion region)
      {
          throw new NotImplementedException();
      }

      public void StopRanging(IList<BeaconRegion> regions)
      {
          throw new NotImplementedException();
      }

      public void StopRangingAllRegions()
      {
          throw new NotImplementedException();
      }

      public bool IsAdvertising
      {
          get { return peripheralManager != null && peripheralManager.Advertising; }
      }

      public bool IsMonitoring
      {
          get { return locationManager != null && locationManager.MonitoredRegions != null && locationManager.MonitoredRegions.Count > 0; }

      }

      public bool IsRanging
      {
          get { return locationManager != null && locationManager.RangedRegions != null && locationManager.RangedRegions.Count > 0; }
      }

  

      /// <summary>
      /// Checks if is available for monitoring
      /// </summary>
      /// <returns></returns>
      public async Task<bool> AvailableForMonitoring()
      {
          bool retVal = false;

          if (!CLLocationManager.LocationServicesEnabled)
          {
              string message = string.Format("{0} - {1}", CrossBeacon.Id, "You need to enable Location Services");
              System.Diagnostics.Debug.WriteLine(message);
              //CrossBeacon.BeaconListener.OnError(message);
          }
          else if (CLLocationManager.Status == CLAuthorizationStatus.Denied || CLLocationManager.Status == CLAuthorizationStatus.Restricted)
          {
              string message = string.Format("{0} - {1}", CrossBeacon.Id, "You need to authorize Location Services");
              System.Diagnostics.Debug.WriteLine(message);
              //CrossBeacon.BeaconListener.OnError(message);

          }
          else if (CLLocationManager.Status == CLAuthorizationStatus.NotDetermined)
          {
              var tcs = new TaskCompletionSource<bool>();
              this.locationManager.AuthorizationChanged += (sender, args) => this.OnAuthorizationChanged(tcs, args);
              this.locationManager.RequestAlwaysAuthorization();
  

              retVal = await tcs.Task;
          }else if (UIApplication.SharedApplication.BackgroundRefreshStatus != UIBackgroundRefreshStatus.Available)
          {
              return false;

          } 
          else if (CLLocationManager.IsMonitoringAvailable(typeof(CLRegion)))
          {
              var settings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert
                | UIUserNotificationType.Badge
                | UIUserNotificationType.Sound,
                new NSSet());
              UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

              retVal = true;
          }
          else
          {
              string message = string.Format("{0} - {1}", CrossBeacon.Id, "Not available for monitoring");
              System.Diagnostics.Debug.WriteLine(message);
              //CrossBeacon.BeaconListener.OnError(message);
          }



          return retVal;

      }

      private void OnAuthorizationChanged(TaskCompletionSource<bool> tcs, CLAuthorizationChangedEventArgs e)
      {

          var valid = (
              e.Status == CLAuthorizationStatus.AuthorizedAlways ||
              e.Status == CLAuthorizationStatus.AuthorizedWhenInUse
          );
          tcs.TrySetResult(valid);
      }

     /* protected override void Dispose(bool disposing)
      {
          base.Dispose(disposing);
          if (disposing)
              this.locationManager.Dispose();
      }*/


      #region Beacon Advertisment
      public void StartAdvertising(BeaconRegion region, BeaconAdvertismentPowerLevel powerLevel)
      {
          var power = new NSNumber((int)powerLevel);
          var pData = region.ToNative().GetPeripheralData(power);
          peripheralDelegate = new BTPeripheralDelegate(this,pData);
          peripheralManager = new CBPeripheralManager(peripheralDelegate, DispatchQueue.DefaultGlobalQueue);
          
      }


      public void StopAdvertising()
      {
          if (this.peripheralManager == null)
              return;
         
          this.peripheralManager.StopAdvertising();

          this.peripheralManager = null;
          this.peripheralDelegate = null;

          CrossBeacon.BeaconListener.OnAdvertisingStopped();
      }

      class BTPeripheralDelegate : CBPeripheralManagerDelegate
      {
          NSDictionary peripheralData;
          BeaconImplementation bImplementation;
          internal BTPeripheralDelegate(BeaconImplementation implementation, NSDictionary data)
          {
              bImplementation = implementation;
              peripheralData = data;
          }
          public override void StateUpdated(CBPeripheralManager peripheral)
          {
              switch (peripheral.State)
              {

                  case CBPeripheralManagerState.PoweredOn:
                      if (peripheralData != null)
                      {
                          peripheral.StartAdvertising(peripheralData);
                        
                          CrossBeacon.BeaconListener.OnAdvertisingStarted();
                      }
                      break;

                  case CBPeripheralManagerState.PoweredOff:

                      bImplementation.StopAdvertising();
                      break;


                  case CBPeripheralManagerState.Resetting:
                      break;

                  case CBPeripheralManagerState.Unauthorized:
                      break;

                  case CBPeripheralManagerState.Unsupported:
                      break;

                  case CBPeripheralManagerState.Unknown:
                      break;
              }

          }
      }
      #endregion
  }
}