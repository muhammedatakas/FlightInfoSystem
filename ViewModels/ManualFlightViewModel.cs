using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FlightInfoSystem.Models;
using FlightInfoSystem.Services;
using System.Collections.Generic;
using System.Linq;

namespace FlightInfoSystem.ViewModels
{
    /// <summary>
    /// View model for managing manual flights.
    /// </summary>
    public class ManualFlightViewModel : INotifyPropertyChanged
    {
        private readonly ManualFlightService _manualFlightService;
        private ObservableCollection<ManualFlight> _manualFlights;
        private List<ManualFlight> _allManualFlights; // Store all flights for filtering

        /// <summary>
        /// Collection of manual flights.
        /// </summary>
        public ObservableCollection<ManualFlight> ManualFlights
        {
            get => _manualFlights;
            set
            {
                _manualFlights = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualFlightViewModel"/> class.
        /// </summary>
        /// <param name="manualFlightService">The manual flight service.</param>
        public ManualFlightViewModel(ManualFlightService manualFlightService)
        {
            _manualFlightService = manualFlightService;
            _manualFlights = new ObservableCollection<ManualFlight>();
            _allManualFlights = new List<ManualFlight>();
        }

        /// <summary>
        /// Loads manual flights asynchronously.
        /// </summary>
        public async Task LoadManualFlightsAsync()
        {
            var flights = await _manualFlightService.GetAllManualFlightsAsync();
            _allManualFlights = flights; // Store all flights for filtering
            UpdateManualFlights(_allManualFlights);
        }

        /// <summary>
        /// Adds a manual flight asynchronously.
        /// </summary>
        /// <param name="flight">The manual flight to add.</param>
        public async Task AddManualFlightAsync(ManualFlight flight)
        {
            await _manualFlightService.AddManualFlightAsync(flight);
            await LoadManualFlightsAsync();
        }

        /// <summary>
        /// Updates a manual flight asynchronously.
        /// </summary>
        /// <param name="flight">The manual flight to update.</param>
        public async Task UpdateManualFlightAsync(ManualFlight flight)
        {
            await _manualFlightService.UpdateManualFlightAsync(flight);
            await LoadManualFlightsAsync();
        }

        /// <summary>
        /// Deletes a manual flight asynchronously.
        /// </summary>
        /// <param name="flightId">The ID of the manual flight to delete.</param>
        public async Task DeleteManualFlightAsync(int flightId)
        {
            await _manualFlightService.DeleteManualFlightAsync(flightId);
            await LoadManualFlightsAsync();
        }

        /// <summary>
        /// Filters the flights based on the given query and criteria.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="criteria">The filter criteria.</param>
        public void FilterFlights(string query, string criteria)
        {
            var filteredFlights = _allManualFlights;

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.ToLower();
                switch (criteria)
                {
                    case "Flight Number":
                        filteredFlights = filteredFlights.Where(f => f.FlightNumber.ToLower().Contains(query)).ToList();
                        break;
                    case "Airline":
                        filteredFlights = filteredFlights.Where(f => f.Airline.ToLower().Contains(query)).ToList();
                        break;
                    case "Departure":
                        filteredFlights = filteredFlights.Where(f => f.Departure.ToLower().Contains(query)).ToList();
                        break;
                    case "Arrival":
                        filteredFlights = filteredFlights.Where(f => f.Arrival.ToLower().Contains(query)).ToList();
                        break;
                    case "Status":
                        filteredFlights = filteredFlights.Where(f => f.Status.ToLower().Contains(query)).ToList();
                        break;
                }
            }

            UpdateManualFlights(filteredFlights);
        }

        private void UpdateManualFlights(IEnumerable<ManualFlight> flights)
        {
            ManualFlights.Clear();
            foreach (var flight in flights)
            {
                ManualFlights.Add(flight);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
