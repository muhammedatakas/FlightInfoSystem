using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FlightInfoSystem.Models;

namespace FlightInfoSystem.Services
{
    /// <summary>
    /// Provides services for retrieving flight information from an external API.
    /// </summary>
    public class FlightService : IFlightService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the FlightService class.
        /// </summary>
        public FlightService()
        {
            _httpClient = new HttpClient();
            _apiKey = "0ee79c5412f79c0adb316826a282de71"; // Consider moving this to a configuration file
        }

        /// <summary>
        /// Retrieves flight information for a given flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number to search for.</param>
        /// <returns>A list of Flight objects containing the flight information.</returns>
        public async Task<List<Flight>> GetFlightInfoAsync(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                throw new ArgumentException("Flight number cannot be empty.", nameof(flightNumber));
            }

            string apiUrl = $"http://api.aviationstack.com/v1/flights?access_key={_apiKey}&flight_number={flightNumber}";
            
            try
            {
                System.Diagnostics.Debug.WriteLine($"Fetching data from: {apiUrl}");
                var response = await _httpClient.GetStringAsync(apiUrl);
                System.Diagnostics.Debug.WriteLine($"API Response: {response}");
                
                var flightData = JsonConvert.DeserializeObject<FlightData>(response);
                return flightData?.Data ?? new List<Flight>();
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP Request error: {e.Message}");
                throw new Exception("Error occurred while fetching flight data.", e);
            }
            catch (JsonException e)
            {
                System.Diagnostics.Debug.WriteLine($"JSON parsing error: {e.Message}");
                throw new Exception("Error occurred while parsing flight data.", e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error: {e.Message}");
                throw new Exception("An unexpected error occurred while retrieving flight information.", e);
            }
        }
    }
}