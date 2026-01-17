using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OGTTrust.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MonitoringService _service;

        public MainViewModel(MonitoringService service)
        {
            _service = service;
            StartStopCommand = new RelayCommand(_ => ToggleStartStop());
            ExitCommand = new RelayCommand(_ => System.Windows.Application.Current.Shutdown());

            // subscribe to service events
            try { _service.SampleProcessed += (s, human) => { TotalSamples = _service.TotalSamples; SuspiciousCount = _service.SuspiciousCount; }; } catch { }
            try { _service.DeviceAttached += d => { }; } catch { }
        }

        private long _totalSamples;
        public long TotalSamples { get => _totalSamples; set { _totalSamples = value; OnPropertyChanged(); } }

        private long _suspiciousCount;
        public long SuspiciousCount { get => _suspiciousCount; set { _suspiciousCount = value; OnPropertyChanged(); } }

        public ICommand StartStopCommand { get; }
        public ICommand ExitCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void ToggleStartStop()
        {
            try
            {
                if (_service.IsRunning)
                    _service.Stop();
                else
                    _service.Start();

                OnPropertyChanged(nameof(IsRunning));
            }
            catch { }
        }

        public bool IsRunning => _service?.IsRunning ?? false;
    }
}
