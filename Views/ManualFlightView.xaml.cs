using System.Windows;
using FlightInfoSystem.ViewModels;
using FlightInfoSystem.Services;
using System;
using FlightInfoSystem.Models;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;

namespace FlightInfoSystem.Views
{
    public partial class ManualFlightView : Window
    {
        private readonly ManualFlightViewModel _viewModel;

        public ManualFlightView()
        {
            InitializeComponent();
            _viewModel = new ManualFlightViewModel(new ManualFlightService(@"Server=localhost\SQLEXPRESS;Database=Logins;Trusted_Connection=True;"));
            DataContext = _viewModel;
            Loaded += ManualFlightView_Loaded;
            SetPlaceholders();
        }

        // Sets the initial placeholders for the text boxes
        private void SetPlaceholders()
        {
            SetPlaceholder(FlightNumberTextBox);
            SetPlaceholder(AirlineTextBox);
            SetPlaceholder(DepartureTextBox);
            SetPlaceholder(ArrivalTextBox);
            SetPlaceholder(StatusTextBox);
        }

        // Sets the placeholder for a specific text box
        private void SetPlaceholder(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        // Loads the manual flights when the view is loaded
        private async void ManualFlightView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.LoadManualFlightsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading flights: {ex.Message}");
            }
        }

        // Adds a new manual flight
        private async void AddFlightButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidFlightInput())
            {
                var flight = new ManualFlight
                {
                    FlightNumber = FlightNumberTextBox.Text,
                    Airline = AirlineTextBox.Text,
                    Departure = DepartureTextBox.Text,
                    Arrival = ArrivalTextBox.Text,
                    Status = StatusTextBox.Text
                };
                await _viewModel.AddManualFlightAsync(flight);
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Please enter all flight details.");
            }
        }

        // Updates an existing manual flight
        private async void UpdateFlightButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidFlightInput())
            {
                if (sender is Button button && button.Tag is int flightId)
                {
                    var flight = new ManualFlight
                    {
                        Id = flightId,
                        FlightNumber = FlightNumberTextBox.Text,
                        Airline = AirlineTextBox.Text,
                        Departure = DepartureTextBox.Text,
                        Arrival = ArrivalTextBox.Text,
                        Status = StatusTextBox.Text
                    };
                    await _viewModel.UpdateManualFlightAsync(flight);
                    ClearInputFields();
                }
            }
            else
            {
                MessageBox.Show("Please enter all flight details.");
            }
        }

        // Deletes a manual flight
        private async void DeleteFlightButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int flightId)
            {
                await _viewModel.DeleteManualFlightAsync(flightId);
            }
        }

        // Clears the input fields and sets the placeholders
        private void ClearInputFields()
        {
            FlightNumberTextBox.Text = string.Empty;
            AirlineTextBox.Text = string.Empty;
            DepartureTextBox.Text = string.Empty;
            ArrivalTextBox.Text = string.Empty;
            StatusTextBox.Text = string.Empty;
            SetPlaceholders();
        }

        // Checks if the flight input is valid
        private bool IsValidFlightInput()
        {
            return !string.IsNullOrWhiteSpace(FlightNumberTextBox.Text) && FlightNumberTextBox.Text != "Flight Number" &&
                   !string.IsNullOrWhiteSpace(AirlineTextBox.Text) && AirlineTextBox.Text != "Airline" &&
                   !string.IsNullOrWhiteSpace(DepartureTextBox.Text) && DepartureTextBox.Text != "Departure" &&
                   !string.IsNullOrWhiteSpace(ArrivalTextBox.Text) && ArrivalTextBox.Text != "Arrival" &&
                   !string.IsNullOrWhiteSpace(StatusTextBox.Text) && StatusTextBox.Text != "Status";
        }
        
        // Handles the GotFocus event of the text boxes
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        // Handles the LostFocus event of the text boxes
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        // Handles the Click event of the Search button
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = SearchTextBox.Text.ToLower();
            string selectedCriteria = ((ComboBoxItem)SearchCriteriaComboBox.SelectedItem).Content.ToString();
            _viewModel.FilterFlights(searchQuery, selectedCriteria);
        }

        // Filters the flights based on the search query and criteria
        private IEnumerable<ManualFlight> FilterFlights(string query, string criteria)
        {
            var _flights = _viewModel.ManualFlights;
            // Assuming _flights is your original collection of flights
            if (_flights != null)
            {
                switch (criteria)
                {
                    case "Flight Number":
                        return _viewModel.ManualFlights.Where(f => f.FlightNumber.ToLower().Contains(query));
                    case "Airline":
                        return _viewModel.ManualFlights.Where(f => f.Airline.ToLower().Contains(query));
                    case "Departure":
                        return _viewModel.ManualFlights.Where(f => f.Departure.ToLower().Contains(query));
                    case "Arrival":
                        return _viewModel.ManualFlights.Where(f => f.Arrival.ToLower().Contains(query));
                    case "Status":
                        return _viewModel.ManualFlights.Where(f => f.Status.ToLower().Contains(query));
                    default:
                        return _viewModel.ManualFlights;
                }
            }
            else
            {   
                MessageBox.Show("No flights found.");
                return Enumerable.Empty<ManualFlight>();
            }
        }

        // Updates the flight data grid with the filtered flights
        private void UpdateFlightDataGrid(IEnumerable<ManualFlight> flights)
        {
            _viewModel.ManualFlights.Clear();
            foreach (var flight in flights)
            {
                _viewModel.ManualFlights.Add(flight);
            }
        }        
    }
}
