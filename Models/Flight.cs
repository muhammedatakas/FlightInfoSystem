using System.Collections.Generic;

namespace FlightInfoSystem.Models
{
    /// <summary>
    /// Represents the overall structure of flight data retrieved from the API.
    /// </summary>
    public class FlightData
    {
        /// <summary>
        /// Gets or sets the pagination information for the flight data.
        /// </summary>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// Gets or sets the list of flight information.
        /// </summary>
        public List<Flight> Data { get; set; }
    }

    /// <summary>
    /// Represents pagination information for the flight data.
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// Gets or sets the limit of items per page.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the offset of the current page.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the count of items on the current page.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int Total { get; set; }
    }

    /// <summary>
    /// Represents detailed information about a single flight.
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// Gets or sets the date of the flight.
        /// </summary>
        public string Flight_date { get; set; }

        /// <summary>
        /// Gets or sets the status of the flight.
        /// </summary>
        public string Flight_status { get; set; }

        /// <summary>
        /// Gets or sets the departure information.
        /// </summary>
        public Departure Departure { get; set; }

        /// <summary>
        /// Gets or sets the arrival information.
        /// </summary>
        public Arrival Arrival { get; set; }

        /// <summary>
        /// Gets or sets the airline information.
        /// </summary>
        public Airline Airline { get; set; }

        /// <summary>
        /// Gets or sets the flight information.
        /// </summary>
        public Flight_Info Flight_Info { get; set; }
    }

    /// <summary>
    /// Represents departure information for a flight.
    /// </summary>
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

    /// <summary>
    /// Represents arrival information for a flight.
    /// </summary>
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

    /// <summary>
    /// Represents airline information.
    /// </summary>
    public class Airline
    {
        public string Name { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
    }

    /// <summary>
    /// Represents specific flight information.
    /// </summary>
    public class Flight_Info
    {
        public string Number { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
    }
}