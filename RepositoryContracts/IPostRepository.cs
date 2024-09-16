using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    Task<Post> AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(string postId);
    Task<Post> GetSingleAsync(string postId);
    IQueryable<Post> GetMany();
    Task LikePostAsync(string id);
    Task RemoveLikePostAsync(string id);
}