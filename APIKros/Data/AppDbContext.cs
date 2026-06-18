using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints and indexes 
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<Division>()
                .HasIndex(d => new { d.CompanyId, d.Code })
                .IsUnique();

            modelBuilder.Entity<Project>()
                .HasIndex(p => new { p.DivisionId, p.Code })
                .IsUnique();

            modelBuilder.Entity<Department>()
                .HasIndex(d => new { d.ProjectId, d.Code })
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => new { e.CompanyId, e.Email })
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => new { e.CompanyId, e.EmployeeNumber })
                .IsUnique();
            
            

            // Configure relationships and foreign keys of hierarchical structure

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Company)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Division>()
                .HasOne(d => d.Company)
                .WithMany(c => c.Divisions)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Division)
                .WithMany(d => d.Projects)
                .HasForeignKey(p => p.DivisionId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Departments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();


           

           // Configure relationships for Manager and Director properties nullable, onDelete will be set null, can be assigned later. 
           modelBuilder.Entity<Company>()
                .HasOne(c => c.Manager)
                .WithMany()
                .HasForeignKey(c => c.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);


           modelBuilder.Entity<Division>()
               .HasOne(d => d.Manager)
               .WithMany()
               .HasForeignKey(d => d.ManagerId)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany()
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);
               

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);
}
    }
}
