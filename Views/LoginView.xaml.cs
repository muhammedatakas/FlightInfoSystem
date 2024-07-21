using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using FlightInfoSystem.Services;

namespace FlightInfoSystem.Views
{
    public partial class LoginView : Window
    {
        private readonly AuthService _authService;

        public LoginView()
        {
            InitializeComponent();
            _authService = new AuthService(@"Server=localhost\SQLEXPRESS;Database=Logins;Trusted_Connection=True;");

            // Set initial placeholder for PasswordBox
            PasswordInputField.Password = "Enter Password";
            PasswordInputField.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = InputField.Text;
            string password = PasswordInputField.Password;

            try
            {
                bool isLoggedIn = await _authService.LoginUserAsync(username, password);
                if (isLoggedIn)
                {
                    OpenMainWindow();
                }
                else
                {
                    ShowMessage("Login failed. Please check your credentials.", "Login Failed");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error during login");
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = InputField.Text;
            string password = PasswordInputField.Password;

            try
            {
                bool isRegistered = await _authService.RegisterUserAsync(username, password);
                ShowMessage(isRegistered ? "User registered successfully!" : "Registration failed. Ensure the username is at least 5 characters and the password meets the criteria.", isRegistered ? "Registration Success" : "Registration Failed");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error during registration");
            }
        }

        private void OpenMainWindow()
        {
            try
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error opening main window");
            }
        }

        private void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void HandleError(Exception ex, string context)
        {
            if (ex != null)
            {
                Debug.WriteLine($"{context}: {ex}");
                MessageBox.Show($"{context}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PasswordInputField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordInputField.Password == "Enter Password")
            {
                PasswordInputField.Password = string.Empty;
                PasswordInputField.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void PasswordInputField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordInputField.Password))
            {
                PasswordInputField.Password = "Enter Password";
                PasswordInputField.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
