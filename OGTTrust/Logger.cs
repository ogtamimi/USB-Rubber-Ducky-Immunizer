using System;

namespace OGTTrust
{
    public static class Logger
    {
        public static event Action<string>? Logged;

        public static void Log(string message)
        {
            try
            {
                Logged?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");
            }
            catch { }
        }
    }
}
