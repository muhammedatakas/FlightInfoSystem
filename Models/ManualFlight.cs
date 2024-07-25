namespace FlightInfoSystem.Models
{
    /// <summary>
    /// Represents a manually entered flight in the system.
    /// </summary>
    public class ManualFlight
    {
        /// <summary>
        /// Gets or sets the unique identifier for the flight.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        public string FlightNumber { get; set; }

        /// <summary>
        /// Gets or sets the airline name.
        /// </summary>
        public string Airline { get; set; }

        /// <summary>
        /// Gets or sets the departure information.
        /// </summary>
        public string Departure { get; set; }

        /// <summary>
        /// Gets or sets the arrival information.
        /// </summary>
        public string Arrival { get; set; }

        /// <summary>
        /// Gets or sets the status of the flight.
        /// </summary>
        public string Status { get; set; }
    }
}