using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
    }
}
