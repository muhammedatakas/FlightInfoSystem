using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FlightInfoSystem.Models;

namespace FlightInfoSystem.Services
{
    /// <summary>
    /// Provides services for managing manual flight entries in the database.
    /// </summary>
    public class ManualFlightService
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the ManualFlightService class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public ManualFlightService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Retrieves all manual flights from the database.
        /// </summary>
        /// <returns>A list of ManualFlight objects.</returns>
        public async Task<List<ManualFlight>> GetAllManualFlightsAsync()
        {
            var flights = new List<ManualFlight>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM ManualFlights";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        flights.Add(new ManualFlight
                        {
                            Id = reader.GetInt32(0),
                            FlightNumber = reader.GetString(1),
                            Airline = reader.GetString(2),
                            Departure = reader.GetString(3),
                            Arrival = reader.GetString(4),
                            Status = reader.GetString(5)
                        });
                    }
                }
            }

            return flights;
        }

        /// <summary>
        /// Adds a new manual flight to the database.
        /// </summary>
        /// <param name="flight">The ManualFlight object to add.</param>
        public async Task AddManualFlightAsync(ManualFlight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(flight));
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO ManualFlights (FlightNumber, Airline, Departure, Arrival, Status) VALUES (@FlightNumber, @Airline, @Departure, @Arrival, @Status)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FlightNumber", flight.FlightNumber);
                    command.Parameters.AddWithValue("@Airline", flight.Airline);
                    command.Parameters.AddWithValue("@Departure", flight.Departure);
                    command.Parameters.AddWithValue("@Arrival", flight.Arrival);
                    command.Parameters.AddWithValue("@Status", flight.Status);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Updates an existing manual flight in the database.
        /// </summary>
        /// <param name="flight">The ManualFlight object to update.</param>
        public async Task UpdateManualFlightAsync(ManualFlight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(flight));
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE ManualFlights SET FlightNumber = @FlightNumber, Airline = @Airline, Departure = @Departure, Arrival = @Arrival, Status = @Status WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", flight.Id);
                    command.Parameters.AddWithValue("@FlightNumber", flight.FlightNumber);
                    command.Parameters.AddWithValue("@Airline", flight.Airline);
                    command.Parameters.AddWithValue("@Departure", flight.Departure);
                    command.Parameters.AddWithValue("@Arrival", flight.Arrival);
                    command.Parameters.AddWithValue("@Status", flight.Status);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Deletes a manual flight from the database.
        /// </summary>
        /// <param name="flightId">The ID of the flight to delete.</param>
        public async Task DeleteManualFlightAsync(int flightId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM ManualFlights WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", flightId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}