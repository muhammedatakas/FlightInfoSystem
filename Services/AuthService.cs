using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace FlightInfoSystem.Services
{
    /// <summary>
    /// Provides authentication services for user registration and login.
    /// </summary>
    public class AuthService
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the AuthService class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public AuthService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="username">The username for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>True if registration was successful, false otherwise.</returns>
        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }

            try
            {
                if (!IsValidPassword(password) || username.Length < 5)
                {
                    Debug.WriteLine("Invalid password or username length");
                    return false;
                }

                string hashedPassword = ComputeSha256Hash(password);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RegisterUserAsync: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Authenticates a user's login credentials.
        /// </summary>
        /// <param name="username">The username of the user trying to log in.</param>
        /// <param name="password">The password of the user trying to log in.</param>
        /// <returns>True if login was successful, false otherwise.</returns>
        public async Task<bool> LoginUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }

            try
            {
                string hashedPassword = ComputeSha256Hash(password);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        int? result = await command.ExecuteScalarAsync() as int?;
                        int userCount = result ?? 0;
                        return userCount > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoginUserAsync: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Computes the SHA256 hash of the input string.
        /// </summary>
        /// <param name="rawData">The input string to hash.</param>
        /// <returns>The computed hash as a hexadecimal string.</returns>
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Validates the password against defined criteria.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if the password is valid, false otherwise.</returns>
        private static bool IsValidPassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsLetter);
        }
    }
}