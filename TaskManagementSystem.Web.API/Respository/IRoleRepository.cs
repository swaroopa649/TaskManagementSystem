using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(long id);
        Task<Role?> GetByCodeAsync(string code);
        Task<bool> ExistsByCodeAsync(string code, long? excludeId = null);
        Task<Role> AddAsync(Role role);
        Task UpdateAsync(Role role);
    }
}
