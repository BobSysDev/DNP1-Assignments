using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        List<Comment> comments = GetManyAsync().ToList();
        if (comments.Any())
        {
            var result = comments.Max(c => Int32.Parse(c.Id.Substring(1, c.Id.Length - 1))) + 1;
            comment.Id = "C" + result;
        }
        else
        {
            comment.Id = "C1";
        }

        EntityEntry<Comment> entityEntry = await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(Comment comment)
    {
        if (!(await ctx.Comments.AnyAsync(c => c.Id == comment.Id)))
        {
            throw new Exception($"Comment with id {comment.Id} not found");
        }
        
        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteCascadeAsync(string id)
    {
        List<Comment> allComments = GetManyAsync().ToList();
        await DeleteCascade(id, allComments);
    }

    private async Task DeleteCascade(string parentId, List<Comment> comments)
    {
        List<Comment> children = [];
            foreach (var comment in comments)
            {
                    if (comment.ParentId == parentId)
                    {
                        children.Add(comment);
                    }
            }

            if (children.Count > 0)
            {
                    foreach (var child in children)
                    {
                        await DeleteCascade(child.Id, comments);
                    }
            }

            await DeleteAsync(parentId);
    }

    public async Task DeleteAsync(string id)
    {
        Comment existing = await GetSingleAsync(id);
        ctx.Comments.Remove(existing);
        await ctx.SaveChangesAsync();
    }
    
    public async Task<Comment> GetSingleAsync(string id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (existing == null)
        {
            throw new Exception($"Comment with id {id} not found"); 
        }
        return existing;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return ctx.Comments.AsQueryable();
    }

    public async Task LikeCommentAsync(string id)
    {
        Comment commentToLike = await GetSingleAsync(id);
        commentToLike.Likes ++;
        await UpdateAsync(commentToLike);
    }

    public async Task RemoveLikeCommentAsync(string id)
    {
        Comment commentToLike = await GetSingleAsync(id);
        commentToLike.Likes --;
        await UpdateAsync(commentToLike);
    }
}