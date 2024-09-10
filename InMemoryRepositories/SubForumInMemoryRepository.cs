using Entities;

namespace InMemoryRepositories;

public class SubForumInMemoryRepository
{
    private List<SubForum> subForums;

    public Task<SubForum> AddAsync(SubForum subForum)
    {
        subForum.Id = subForums.Any()
            ? subForums.Max(c => c.Id) + 1
            : 1;
        subForums.Add(subForum);
        return Task.FromResult(subForum);
    }

    public Task UpdateAsync(SubForum subForum)
    {
        SubForum? existingSubForum = subForums.SingleOrDefault(c => c.Id == subForum.Id);
        if (existingSubForum is null)
            throw new InvalidOperationException($"SubForum with ID '{subForum.Id}' not found");

        subForums.Remove(existingSubForum);
        subForums.Add(subForum);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        SubForum? subForumToRemove = subForums.SingleOrDefault(c => c.Id == id);
        if (subForumToRemove is null) throw new InvalidOperationException($"SubForum with ID '{id}' not found");

        subForums.Remove(subForumToRemove);
        return Task.CompletedTask;
    }

    public Task<SubForum> GetSingleAsync(int id)
    {
        SubForum? subForumToRetrieve = subForums.SingleOrDefault(c => c.Id == id);
        if (subForumToRetrieve is null) throw new InvalidOperationException($"SubForum with ID '{id}' not found");

        return Task.FromResult(subForumToRetrieve);
    }

    public IQueryable<SubForum> GetManyAsync()
    {
        return subForums.AsQueryable();
    }
}