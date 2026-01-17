using System;
using System.Management;

namespace OGTTrust
{
    public class DeviceWatcher : IDisposable
    {
        private ManagementEventWatcher? _watcher;

        public event Action<DeviceInfo>? DeviceAttached;

        public void Start()
        {
            try
            {
                var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
                _watcher = new ManagementEventWatcher(query);
                _watcher.EventArrived += Watcher_EventArrived;
                _watcher.Start();
                Logger.Log("DeviceWatcher started");
            }
            catch (Exception ex)
            {
                Logger.Log("DeviceWatcher start failed: " + ex.Message);
            }
        }

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            // Minimal: emit a generic attach event. Detailed device parsing requires SetupAPI.
            var info = new DeviceInfo
            {
                DeviceId = "unknown",
                Vid = "?",
                Pid = "?",
                Manufacturer = "?",
                Timestamp = DateTime.UtcNow
            };

            DeviceAttached?.Invoke(info);
            Logger.Log("Device attached (generic event)");
        }

        public void Stop()
        {
            try
            {
                _watcher?.Stop();
                _watcher?.Dispose();
                _watcher = null;
                Logger.Log("DeviceWatcher stopped");
            }
            catch (Exception ex)
            {
                Logger.Log("DeviceWatcher stop failed: " + ex.Message);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
