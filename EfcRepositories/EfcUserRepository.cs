using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Entities;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext _ctx;

    public EfcUserRepository(AppContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await _ctx.Users.ToListAsync();

        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;

        EntityEntry<User> entityEntry = await _ctx.Users.AddAsync(user);
        await _ctx.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task UpdateAsync(User user)
    {
        if (!await _ctx.Users.AnyAsync(u => u.Id == user.Id))
        {
            throw new InvalidOperationException($"User with ID {user.Id} not found.");
        }

        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User existing = await GetSingleAsync(id);
        _ctx.Users.Remove(existing);
        await _ctx.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? user = await _ctx.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {id} not found.");
        }
        return user;
    }

    public IQueryable<User> GetMany()
    {
        return _ctx.Users.AsQueryable();
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        User? user = await _ctx.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new InvalidOperationException($"User with username '{username}' not found.");
        }
        return user;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await GetSingleAsync(id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _ctx.Users.ToListAsync();
    }
}