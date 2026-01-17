# OGTTrust - USB Device Protection Monitor

**OGTTrust** is a modern desktop application that monitors USB device activity and typing behavior to detect suspicious events and potential threats. Built with WPF and .NET 10, it provides real-time protection against USB-based attacks and automated keystroke injection devices.

## 🛡️ Features

- **USB Device Monitoring** - Real-time detection of USB device attach/detach events
- **Keystroke Analysis** - Samples typing patterns and detects anomalous behavior  
- **Modern WPF UI** - Dark theme with smooth animations and rounded controls
- **Async Monitoring Service** - Efficient buffered event processing using channels
- **System Tray Integration** - Minimize to tray with context menu
- **Persistent Logging** - Event logs saved to %LOCALAPPDATA%\OGTTrust\
- **Background Service Mode** - Run `OGTTrust.exe --service` for headless monitoring
- **Modern GUI Installer** - Easy installation with custom folder selection

## 🚀 Quick Start

### Download Installer 
- Download `OGTTrustInstaller.exe`
- Run the installer and follow the prompts
- Choose installation folder and create Start Menu shortcut
- Click Install

## 💻 Usage

### GUI Mode (Default)
```bash
OGTTrust.exe
```
- Starts the application with the WPF UI
- Monitoring begins automatically
- Minimize to tray or exit from window controls

### Service Mode (Headless)
```bash
OGTTrust.exe --service
```
- Runs monitoring without UI (background mode)
- Press Ctrl+C to stop
- Useful for server deployments

## 🏗️ Architecture

### Core Components

**MonitoringService**
- Async event processor using `System.Threading.Channels`
- Thread-safe metrics via `Interlocked` operations
- Graceful error handling and fallback behavior
- Events: `DeviceAttached`, `SampleProcessed`, `AlertRaised`

**DeviceWatcher**
- WMI-based USB device monitoring
- Real-time attach/detach event detection

**KeystrokeMonitor**
- Low-level keyboard hook (WH_KEYBOARD_LL)
- Keystroke sampling with timestamp buffering
- Safe initialization with error recovery

**TypingAnalyzer**
- Keystroke pattern analysis
- Anomaly detection for suspicious typing

**MainWindow (WPF)**
- Modern dark theme UI
- Real-time metrics display
- Start/Stop button with MVVM binding
- Tray icon integration
- Welcome dialog on first run

### Project Structure
```
OGTTrust/
├── App.xaml/.cs              # Application entry point
├── MainWindow.xaml/.cs       # Main UI window
├── MonitoringService.cs      # Core monitoring logic
├── DeviceWatcher.cs          # USB device monitoring
├── KeystrokeMonitor.cs       # Keyboard hook
├── TypingAnalyzer.cs         # Pattern analysis
├── DeviceInfo.cs             # Device information model
├── Logger.cs                 # Logging service
├── RelayCommand.cs           # MVVM command implementation
├── BoolToStartStopConverter.cs # UI value converter
├── ViewModels/
│   └── MainViewModel.cs      # MVVM ViewModel
├── Themes/
│   └── OGTTheme.xaml         # Dark theme resources
└── Properties/
    └── Resources.resx        # Application resources
```

## ⚙️ Configuration

**Log File**: `%LOCALAPPDATA%\OGTTrust\ogttrust.log`

All application events and monitoring data are logged with timestamps.

## 🛠️ Development

### Prerequisites
- .NET 10 SDK
- Windows 10 or later
- Visual Studio 2024 or VS Code (optional)

### Build from Source

```bash
# Clone the repository
git clone https://github.com/yourusername/ogttrust.git
cd ogttrust

# Build the application
dotnet build

# Run the application
dotnet run --project OGTTrust

# Build the installer
dotnet build Installer -c Release

# Run the installer
.\Installer\bin\Release\net10.0-windows\OGTTrustInstaller.exe
```

## 🔧 Technology Stack

- **Framework**: .NET 10 (net10.0-windows)
- **UI**: WPF (Windows Presentation Foundation)
- **Architecture**: MVVM with Data Binding
- **Monitoring**: WMI for USB events, Low-level keyboard hook
- **Async Processing**: Channels for buffered event handling
- **Theming**: Centralized resource dictionary with dark theme
- **Dependencies**: 
  - System.Management (8.0.0)
  - Microsoft.Toolkit.Uwp.Notifications (7.1.3)

## 📊 Monitoring Details

### USB Device Detection
- Monitors Win32_DeviceChangeEvent for device attach/detach
- Captures device information including VID/PID when available
- Logs all USB events for security analysis

### Keystroke Analysis
- Samples keystrokes in batches of 20
- Analyzes typing patterns using statistical methods
- Detects suspicious behavior based on:
  - Keys per second rate (>15 KPS)
  - Timing variance (low variance with high speed)
  - Backspace usage patterns

### Alert System
- Real-time notifications for suspicious activity
- Persistent logging for forensic analysis
- Configurable alert thresholds

## 🚨 Security Considerations

- **Administrator Privileges**: May be required for low-level keyboard hooks
- **USB Access**: Monitors all USB device events system-wide
- **Logging**: Sensitive information may be logged (review before sharing logs)
- **Performance**: Minimal impact on system performance

## 🐛 Troubleshooting

**App won't start?**
- Check logs at `%LOCALAPPDATA%\OGTTrust\ogttrust.log`
- Ensure .NET 10 Runtime is installed: `dotnet --version`
- Try running with elevated privileges

**Keyboard monitoring not working?**
- Low-level hooks may require admin rights on some systems
- Check logs for "KeystrokeMonitor failed" message
- App will continue monitoring USB events regardless

**Installer crashes?**
- Run as Administrator if installing to Program Files
- Ensure destination folder is writable
- Check disk space

## 🤝 Contributing

Contributions are welcome! Areas for enhancement:
- WinUI 3 port for full Fluent design language
- Persistent user preferences storage
- Device whitelist/blacklist functionality
- Integration with threat databases
- Multilingual UI support
- Dark/Light theme toggle

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Omar Al Tamimi**
- **LinkedIn**: [linkedin.com/in/ogtamimi/](https://www.linkedin.com/in/ogtamimi/)
- **GitHub**: [github.com/ogtamimi](https://github.com/ogtamimi)

## 📞 Support & Contact

- **Issues**: [GitHub Issues](../../issues)
- **Email**: [Contact via LinkedIn](https://www.linkedin.com/in/ogtamimi/)

---

**Stay secure with OGTTrust** 🔒

Made with ❤️ by Omar Al Tamimi
