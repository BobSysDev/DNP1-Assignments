using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(string id);
    Task DeleteCascadeAsync(string id);
    Task<Comment> GetSingleAsync(string id);
    IQueryable<Comment> GetManyAsync();
    Task LikeCommentAsync(string id);
    Task RemoveLikeCommentAsync(string id);

}