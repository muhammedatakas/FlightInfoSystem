using System.Threading.Tasks;
namespace FlightInfoSystem.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(string username, string password);
        Task<bool> LoginUserAsync(string username, string password);
    }
}