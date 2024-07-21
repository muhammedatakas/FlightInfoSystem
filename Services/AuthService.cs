using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace FlightInfoSystem.Services
{
    public class AuthService
    {
        private readonly string _connectionString;

        public AuthService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> RegisterUserAsync(string username, string password)
        {
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

        public async Task<bool> LoginUserAsync(string username, string password)
        {
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

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static bool IsValidPassword(string password)
        {
            if (password.Length < 8 || !password.Any(char.IsDigit) || !password.Any(char.IsLetter))
            {
                return false;
            }
            return true;
        }
    }
}
