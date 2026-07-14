using APIKros.Data;
using APIKros.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public interface IAuthRepository : IRepository<AuthUser, int>
{
    Task<AuthUser?> GetByEmailAsync(string email);
    Task<AuthUser?> GetByEmailWithRolesAsync(string email);
    Task<bool> EmailExistsAsync(string email);
}


public class AuthRepository : Repository<AuthUser, int>, IAuthRepository
{
    public AuthRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<AuthUser?> GetByEmailAsync(string email)
    {
        return await DbContext.AuthUsers.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();
    }


    public async Task<AuthUser?> GetByEmailWithRolesAsync(string email)
    {
        return await DbContext.AuthUsers
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await DbContext.AuthUsers
            .AnyAsync(u => u.Email == email);
    }
}