using System;
using System.IO;
using System.Windows;

namespace OGTTrust
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Support two modes:
            //  - Tray UI (default)
            //  - Background service mode: run with --service
            bool serviceMode = e.Args != null && e.Args.Length > 0 && e.Args[0] == "--service";

            if (serviceMode)
            {
                // Run as headless background service-like process
                var svc = new MonitoringService();
                svc.Start();

                var mre = new System.Threading.ManualResetEvent(false);
                Console.CancelKeyPress += (s, e) => {
                    e.Cancel = true;
                    mre.Set();
                };

                Console.WriteLine("Service mode running. Press Ctrl+C to exit.");
                mre.WaitOne();

                svc.Stop();
                svc.Dispose();
                
                Shutdown();
            }
            else
            {
                // Show main window directly
                try
                {
                    var mainWindow = new MainWindow();
                    MainWindow = mainWindow;
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    // If anything fails, show a simple window
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                    try
                    {
                        var simpleWindow = new System.Windows.Window();
                        simpleWindow.Title = "OGTTrust - Error Recovery";
                        simpleWindow.Height = 200;
                        simpleWindow.Width = 300;
                        simpleWindow.Content = "Application started with errors. Please check the console for details.";
                        simpleWindow.Show();
                    }
                    catch { }
                }
            }
        }
    }
}
