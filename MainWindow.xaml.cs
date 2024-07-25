using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FlightInfoSystem.ViewModels;
using FlightInfoSystem.Views;

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

        // Event handler for the search button click
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

        // Event handler for the flight number text changed
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

        // Event handler for the "Manage Manual Flights" button click
        private void ManageManualFlights_Click(object sender, RoutedEventArgs e)
        {
            var manualFlightView = new ManualFlightView();
            manualFlightView.Show();
        }
    }
}
