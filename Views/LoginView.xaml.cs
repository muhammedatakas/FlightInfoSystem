using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using FlightInfoSystem.Services;

namespace FlightInfoSystem.Views
{
    /// <summary>
    /// Represents the login view of the application.
    /// </summary>
    public partial class LoginView : Window
    {
        private readonly AuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginView"/> class.
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
            _authService = new AuthService(@"Server=localhost\SQLEXPRESS;Database=Logins;Trusted_Connection=True;");

            // Set initial placeholder for PasswordBox
            PasswordInputField.Password = "Enter Password";
            PasswordInputField.Foreground = new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// Handles the click event of the LoginButton.
        /// </summary>
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

        /// <summary>
        /// Handles the click event of the RegisterButton.
        /// </summary>
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

        /// <summary>
        /// Opens the main window of the application.
        /// </summary>
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

        /// <summary>
        /// Shows a message box with the specified message and caption.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The caption of the message box.</param>
        private void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handles an error by logging it and displaying an error message box.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="context">The context in which the error occurred.</param>
        private void HandleError(Exception ex, string context)
        {
            if (ex != null)
            {
                Debug.WriteLine($"{context}: {ex}");
                MessageBox.Show($"{context}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the PasswordInputField.
        /// </summary>
        private void PasswordInputField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordInputField.Password == "Enter Password")
            {
                PasswordInputField.Password = string.Empty;
                PasswordInputField.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// Handles the LostFocus event of the PasswordInputField.
        /// </summary>
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
