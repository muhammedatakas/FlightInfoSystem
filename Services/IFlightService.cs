namespace FlightInfoSystem.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FlightInfoSystem.Models;

    public interface IFlightService
    {
        Task<List<Flight>> GetFlightInfoAsync(string flightNumber);
    }
}