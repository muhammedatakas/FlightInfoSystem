using System.Collections.Generic;

namespace FlightInfoSystem.Models
{
    public class FlightData
{
    public Pagination Pagination { get; set; }
    public List<Flight> Data { get; set; }
}

public class Pagination
{
    public int Limit { get; set; }
    public int Offset { get; set; }
    public int Count { get; set; }
    public int Total { get; set; }
}

public class Flight
{
    public string Flight_date { get; set; }
    public string Flight_status { get; set; }
    public Departure Departure { get; set; }
    public Arrival Arrival { get; set; }
    public Airline Airline { get; set; }
    public Flight_Info Flight_Info { get; set; }
}

public class Departure
{
    public string Airport { get; set; }
    public string Timezone { get; set; }
    public string Iata { get; set; }
    public string Icao { get; set; }
    public string Terminal { get; set; }
    public string Gate { get; set; }
    public string Delay { get; set; }
    public string Scheduled { get; set; }
    public string Estimated { get; set; }
    public string Actual { get; set; }
    public string Estimated_runway { get; set; }
    public string Actual_runway { get; set; }
}

public class Arrival
{
    public string Airport { get; set; }
    public string Timezone { get; set; }
    public string Iata { get; set; }
    public string Icao { get; set; }
    public string Terminal { get; set; }
    public string Gate { get; set; }
    public string Baggage { get; set; }
    public string Delay { get; set; }
    public string Scheduled { get; set; }
    public string Estimated { get; set; }
    public string Actual { get; set; }
    public string Estimated_runway { get; set; }
    public string Actual_runway { get; set; }
}

public class Airline
{
    public string Name { get; set; }
    public string Iata { get; set; }
    public string Icao { get; set; }
}

public class Flight_Info
{
    public string Number { get; set; }
    public string Iata { get; set; }
    public string Icao { get; set; }
}
}
