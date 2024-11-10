using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
 
 private List<User> users = new List<User>();
 
    public Task<User> AddAsync(User user)
    {
        
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null) throw new InvalidOperationException($"User({user.Id}) not found");
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null) throw new InvalidOperationException($"User({id}) not found");
        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userToReturn = users.SingleOrDefault(u => u.Id == id);
        // Console.WriteLine($"Attempted to find User with Id {id}, but none was found.");
        if (userToReturn is null) throw new InvalidOperationException($"User({id}) not found");
        return Task.FromResult(userToReturn);
    }

    public Task<User> GetByIdAsync(int id)
    {
        return null;
    }

    public Task<User> GetByUsernameAsync(String username)
    {
        return null;
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
    
    public Task<List<User>> GetAllAsync()
    {
        return Task.FromResult(new List<User>(users));
    }

}