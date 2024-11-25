using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext ctx;

    public EfcPostRepository(AppContext context)
    {
        ctx = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Post> AddAsync(Post post)
    {
        if (post == null)
        {
            throw new ArgumentNullException(nameof(post));
        }

        // Ensure UserId exists
        bool userExists = await ctx.Users.AnyAsync(u => u.Id == post.UserId);
        if (!userExists)
        {
            throw new Exception($"User with ID {post.UserId} does not exist.");
        }
        int newPostId = await ctx.Posts.CountAsync() + 1;  
        post.PostId = newPostId.ToString(); 

        await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return post;
    }


    public async Task UpdateAsync(Post post)
    {
        if (!(await ctx.Posts.AnyAsync(p => p.PostId == post.PostId)))
        {
            throw new Exception($"Post with id {post.PostId} not found");
        }

        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(string postId)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p => p.PostId == postId);
        if (existing == null)
        {
            throw new Exception($"Post with id {postId} not found");
        }

        ctx.Posts.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(string postId)
    {
        Post? post = await ctx.Posts.SingleOrDefaultAsync(p => p.PostId == postId);
        if (post == null)
        {
            throw new Exception($"Post with id {postId} not found");
        }

        return post;
    }

    public IQueryable<Post> GetMany()
    {
        return ctx.Posts.AsQueryable();
    }

    public async Task LikePostAsync(string id)
    {
        Post? post = await ctx.Posts.SingleOrDefaultAsync(p => p.PostId == id);
        if (post == null)
        {
            throw new Exception($"Post with id {id} not found");
        }

        post.Likes++;
        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }

    public async Task RemoveLikePostAsync(string id)
    {
        Post? post = await ctx.Posts.SingleOrDefaultAsync(p => p.PostId == id);
        if (post == null)
        {
            throw new Exception($"Post with id {id} not found");
        }

        post.Likes--;
        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }
}