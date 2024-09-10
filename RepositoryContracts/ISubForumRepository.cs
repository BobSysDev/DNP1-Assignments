using Entities;

namespace RepositoryContracts;

public interface ISubForumRepository
{
    Task<SubForum> AddAsync(SubForum comment);
    Task UpdateAsync(SubForum comment);
    Task DeleteAsync(int id);
    Task<SubForum> GetSingleAsync(int id);
    IQueryable<SubForum> GetManyAsync();
}