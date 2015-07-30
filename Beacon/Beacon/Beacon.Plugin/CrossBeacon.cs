using Beacon.Plugin.Abstractions;
using System;
using System.Diagnostics;

namespace Beacon.Plugin
{
  /// <summary>
  /// Cross platform Beacon implemenations
  /// </summary>
  public class CrossBeacon
  {
    static Lazy<IBeacon> Implementation = new Lazy<IBeacon>(() => CreateBeacon(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Checks if plugin is initialized
    /// </summary>
    public static bool IsInitialized { get { return (BeaconListener != null); } }
    /// <summary>
    /// Beacon state events listener
    /// </summary>
    internal static IBeaconListener BeaconListener { get; private set; }
    /// <summary>
    /// Plugin id
    /// </summary>
    public const string Id = "CrossBeacon";
    public static void Initialize<T>()
      where T : IBeaconListener, new()
    {

        if (BeaconListener == null)
        {

            BeaconListener = (IBeaconListener)Activator.CreateInstance(typeof(T));
            Debug.WriteLine("Beacon plugin initialized.");
        }
        else
        {
            Debug.WriteLine("Beacon plugin already initialized.");
        }


    }
    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IBeacon Current
    {
      get
      {
        if (!CrossBeacon.IsInitialized)
        {
              throw BeaconNotInitializedException();
        }
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static IBeacon CreateBeacon()
    {
#if PORTABLE
        return null;
#else
        return new BeaconImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }

    internal static BeaconNotInitializedException BeaconNotInitializedException()
    {
        string description = string.Format("{0} - {1}", CrossBeacon.Id, "Plugin is not initialized. Should initialize before use with CrossBeacon Initialize method. Example:  CrossBeacon.Initialize<CrossBeaconListener>()");

        return new BeaconNotInitializedException(description);
    }
  }
}
