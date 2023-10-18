using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class UserRepository : ICrudRepository<User>
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User?> CreateAsync(User entity)
    {
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<List<User>?> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int? id)
    {
        return await _context.Users.FindAsync(id);
    }
    
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<User?> UpdateAsync(User entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity != null)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}