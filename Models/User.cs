namespace FlightInfoSystem.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hashed password.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
    }
}