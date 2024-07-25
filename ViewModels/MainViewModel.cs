using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FlightInfoSystem.Models;
using FlightInfoSystem.Services;
using System.Windows;

namespace FlightInfoSystem.ViewModels
{
    /// <summary>
    /// Represents the main view model for the Flight Information System.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FlightService _flightService;
        private ObservableCollection<Flight> _flights;

        /// <summary>
        /// Gets or sets the collection of flights.
        /// </summary>
        public ObservableCollection<Flight> Flights
        {
            get => _flights;
            set
            {
                _flights = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            _flightService = new FlightService();
            _flights = new ObservableCollection<Flight>();
        }

        /// <summary>
        /// Retrieves flight information asynchronously for the specified flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number.</param>
        public async Task GetFlightInfoAsync(string flightNumber)
        {
            var flightInfo = await _flightService.GetFlightInfoAsync(flightNumber);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Flights.Clear();
                foreach (var flight in flightInfo)
                {
                    Flights.Add(flight);
                }
            });
            System.Diagnostics.Debug.WriteLine($"Flight info retrieved and updated. Count: {Flights.Count}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
