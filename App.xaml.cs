using System;
using System.Windows;
using System.Windows.Threading;
using FlightInfoSystem.Views;
using System.Diagnostics;

namespace FlightInfoSystem
{
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                var loginView = new LoginView();
                loginView.Show();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Error during startup");
                this.Shutdown();
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, "Unhandled exception");
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "Fatal exception");
            }
            else
            {
                // Handle the case where ExceptionObject is not an Exception (unlikely, but possible)
                Debug.WriteLine("Fatal exception of unknown type.");
                MessageBox.Show("A fatal exception of unknown type has occurred.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
}

        private void HandleException(Exception ex, string context)
        {
            if (ex != null)
            {
                Debug.WriteLine($"{context}: {ex}");
                MessageBox.Show($"{context}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
