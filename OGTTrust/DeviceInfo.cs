using System;

namespace OGTTrust
{
    public class DeviceInfo
    {
        public string? DeviceId { get; set; }
        public string? Vid { get; set; }
        public string? Pid { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public override string ToString()
        {
            return $"DeviceId={DeviceId} VID={Vid} PID={Pid} Manufacturer={Manufacturer} Time={Timestamp:u}";
        }
    }
}
