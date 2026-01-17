using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OGTTrust
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MonitoringViewModel();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

    public class MonitoringViewModel : INotifyPropertyChanged
    {
        private bool _isRunning;
        private RelayCommand _startStopCommand;

        public MonitoringViewModel()
        {
            _isRunning = true;
            _startStopCommand = new RelayCommand(ToggleMonitoring);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand StartStopCommand
        {
            get => _startStopCommand;
            set => _startStopCommand = value;
        }

        private void ToggleMonitoring(object? parameter)
        {
            IsRunning = !IsRunning;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
