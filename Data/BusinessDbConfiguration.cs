using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Data;

public static class BusinessDbConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        // Configure constraints, indexes and relationships
        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);

            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);

            entity.HasIndex(e => new { e.CompanyId, e.Code }).IsUnique();
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);

            entity.HasIndex(e => new { e.DivisionId, e.Code }).IsUnique();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => new { e.ProjectId, e.Code }).IsUnique();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Title).HasMaxLength(80);
            entity.Property(e => e.CompanyId).IsRequired();

            entity.HasIndex(e => new { e.CompanyId, e.Email }).IsUnique();
            entity.HasIndex(e => new { e.CompanyId, e.EmployeeNumber }).IsUnique();
        });


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