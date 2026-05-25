using FlysphereBackendDotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(long id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(long id);
    }
}
