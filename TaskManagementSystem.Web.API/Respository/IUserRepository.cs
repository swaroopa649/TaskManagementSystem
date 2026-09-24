using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(long id);

        Task<User?> GetByEmailAsync(string email);

        Task<bool> ExistsByEmailAsync(string email, long? excludeId = null);

        Task<bool> RoleExistsAsync(long roleId);

        Task<User> AddAsync(User user);

        Task UpdateAsync(User user);
    }
}
