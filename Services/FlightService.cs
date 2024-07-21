using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FlightInfoSystem.Models;
using System;

namespace FlightInfoSystem.Services
{
    public class FlightService : IFlightService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "0ee79c5412f79c0adb316826a282de71";

    public FlightService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Flight>> GetFlightInfoAsync(string flightNumber)
    {
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
            return new List<Flight>();
        }
        catch (JsonException e)
        {
            System.Diagnostics.Debug.WriteLine($"JSON parsing error: {e.Message}");
            return new List<Flight>();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Unexpected error: {e.Message}");
            return new List<Flight>();
        }
    }
}
}
