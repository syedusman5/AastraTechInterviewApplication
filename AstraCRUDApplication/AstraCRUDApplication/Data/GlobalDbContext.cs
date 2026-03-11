using AstraCRUDApplication.Models.TableModels;
using Microsoft.EntityFrameworkCore;

namespace AstraCRUDApplication.Data
{
    public class GlobalDbContext : DbContext
    {
        public GlobalDbContext(DbContextOptions<GlobalDbContext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Department> Department { get; set; }

    }
}
