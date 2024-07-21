using System.Windows;
using System.Windows.Controls;
using FlightInfoSystem.ViewModels;
using System.Windows.Media;
using System;
namespace FlightInfoSystem
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }
        

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string flightNumber = TxtFlightNumber.Text;
                if (string.IsNullOrWhiteSpace(flightNumber) || flightNumber == "Enter Flight Number")
                {
                    MessageBox.Show("Please enter a flight number.");
                    return;
                }
                await _viewModel.GetFlightInfoAsync(flightNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching flight info: {ex.Message}");
            }
        }


        private void TxtFlightNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtFlightNumber.Text == "Enter Flight Number" && TxtFlightNumber.IsKeyboardFocused)
            {
                TxtFlightNumber.Text = string.Empty;
                TxtFlightNumber.Foreground = new SolidColorBrush(Colors.Black);
            }
            else if (string.IsNullOrEmpty(TxtFlightNumber.Text) && !TxtFlightNumber.IsKeyboardFocused)
            {
                TxtFlightNumber.Text = "Enter Flight Number";
                TxtFlightNumber.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
