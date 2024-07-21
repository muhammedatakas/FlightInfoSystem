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
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FlightService _flightService;
        private ObservableCollection<Flight> _flights;

        public ObservableCollection<Flight> Flights
        {
            get => _flights;
            set
            {
                _flights = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _flightService = new FlightService();
            _flights = new ObservableCollection<Flight>();
        }

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
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
