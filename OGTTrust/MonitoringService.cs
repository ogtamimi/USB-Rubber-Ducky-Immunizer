using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OGTTrust
{
    // Minimal background service controller that starts the same monitoring components used by the UI.
    public class MonitoringService : IDisposable
    {
        private DeviceWatcher? _watcher;
        private KeystrokeMonitor? _keys;
        private Channel<TypingSample>? _channel;
        private CancellationTokenSource? _cts;
        private Task? _processorTask;

        private long _totalSamples;
        private long _suspiciousCount;
        public DateTime? LastAlert { get; private set; }

        public long TotalSamples => Interlocked.Read(ref _totalSamples);
        public long SuspiciousCount => Interlocked.Read(ref _suspiciousCount);
        public bool IsRunning => _cts != null && !_cts.IsCancellationRequested;

        public event Action<DeviceInfo>? DeviceAttached;
        public event Action<TypingSample, bool>? SampleProcessed;
        public event Action<TypingSample>? AlertRaised;

        public void Start()
        {
            if (_cts != null) return;

            Logger.Log("MonitoringService starting");

            _cts = new CancellationTokenSource();
            _channel = Channel.CreateBounded<TypingSample>(new BoundedChannelOptions(1024) { SingleReader = true, SingleWriter = false });

            _watcher = new DeviceWatcher();
            _watcher.DeviceAttached += OnDeviceAttached;
            _watcher.Start();

            try
            {
                _keys = new KeystrokeMonitor();
                _keys.SampleReady += sample => {
                    try { _channel?.Writer.TryWrite(sample); } catch { }
                };
            }
            catch (Exception ex)
            {
                Logger.Log("KeystrokeMonitor failed: " + ex.Message);
                _keys = null;
            }

            _processorTask = Task.Run(() => ProcessorLoopAsync(_cts.Token));

            Logger.Log("MonitoringService started");
        }

        private void OnDeviceAttached(DeviceInfo info)
        {
            Logger.Log("[Service] Attached: " + info);
            DeviceAttached?.Invoke(info);
        }

        private async Task ProcessorLoopAsync(CancellationToken ct)
        {
            var reader = _channel!.Reader;
            try
            {
                while (await reader.WaitToReadAsync(ct).ConfigureAwait(false))
                {
                    while (reader.TryRead(out var sample))
                    {
                        Interlocked.Increment(ref _totalSamples);
                        bool human = TypingAnalyzer.IsHumanLike(sample);
                        SampleProcessed?.Invoke(sample, human);
                        Logger.Log($"[Service] Typing sample processed. HumanLike={human}");
                        if (!human)
                        {
                            Interlocked.Increment(ref _suspiciousCount);
                            LastAlert = DateTime.UtcNow;
                            AlertRaised?.Invoke(sample);
                            Logger.Log("[Service] Suspicious typing detected!");
                        }
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Logger.Log("Processor error: " + ex.Message);
            }
        }

        public void Stop()
        {
            if (_cts == null) return;

            Logger.Log("MonitoringService stopping");

            try { _cts.Cancel(); } catch { }
            try { _processorTask?.Wait(1500); } catch { }
            try { _keys?.Dispose(); } catch { }
            try { _watcher?.Dispose(); } catch { }

            _keys = null;
            _watcher = null;
            _channel = null;
            _processorTask = null;
            _cts.Dispose();
            _cts = null;

            Logger.Log("MonitoringService stopped");
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
