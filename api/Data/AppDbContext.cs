using APIKros.Models;
using APIKros.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Division> Divisions => Set<Division>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<AuthUser> AuthUsers => Set<AuthUser>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            BusinessDbConfiguration.Configure(modelBuilder);
            AuthDbConfiguration.Configure(modelBuilder);
        }
        

       
    }
}
