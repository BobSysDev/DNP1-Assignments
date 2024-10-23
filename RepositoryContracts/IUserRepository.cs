using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<User> GetSingleAsync(int id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByIdAsync(int id);
    IQueryable<User> GetMany();
}