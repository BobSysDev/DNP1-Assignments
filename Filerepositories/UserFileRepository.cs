using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace Filerepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string _filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }
        
    //private List<User> users = new List<User>();
 
    public async Task<User> AddAsync(User user)
    {
        var usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson);

        if (users is null)
        {
            throw new InvalidDataException("Deserializing user list returned NULL");
        }
        
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        
        usersAsJson = JsonSerializer.Serialize(users, options: new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(_filePath, usersAsJson);
        
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson);

        if (users is null)
        {
            throw new InvalidDataException("Deserializing user list returned NULL");
        }

        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null) throw new InvalidOperationException($"User({user.Id}) not found");
        users.Remove(existingUser);
        users.Add(user);
        
        usersAsJson = JsonSerializer.Serialize(users, options: new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        var usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson);

        if (users is null)
        {
            throw new InvalidDataException("Deserializing user list returned NULL");
        }

        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null) throw new InvalidOperationException($"User({id}) not found");
        users.Remove(userToRemove);
        
        usersAsJson = JsonSerializer.Serialize(users, options: new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson);

        if (users is null)
        {
            throw new InvalidDataException("Deserializing user list returned NULL");
        }
        
        User? userToReturn = users.SingleOrDefault(u => u.Id == id);
        //Console.WriteLine($"Attempted to find User with Id {id}, but none was found.");
        if (userToReturn is null) throw new InvalidOperationException($"User({id}) not found");

        return userToReturn;
    }

    public IQueryable<User> GetMany()
    {
        string usersAsJson = File.ReadAllTextAsync(_filePath).Result; 
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!; 
        return users.AsQueryable();
    }
    
        public Task<User> GetByUsernameAsync(string username)
    {
        List<User> users = new List<User>();
        User? userToReturn = users.SingleOrDefault(u => u.Username == username);
        // Console.WriteLine($"Attempted to find User with Id {id}, but none was found.");
        if (userToReturn is null) throw new InvalidOperationException($"User({username}) not found");
        return Task.FromResult(userToReturn);
    }
}