using FlysphereBackendDotnet.Models;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(User user);
Task<object?> LoginAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
    }
}
