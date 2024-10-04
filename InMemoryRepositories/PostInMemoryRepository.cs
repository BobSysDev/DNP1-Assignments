using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = new List<Post>();

    public Task<Post> AddAsync(Post post)
    {
        if (posts.Any())
        {
            var result = posts.Max(p => Int32.Parse(p.PostId.Substring(1, p.PostId.Length - 1))) + 1;
            post.PostId = "P" + result;
        }
        else
        {
            post.PostId = "P1";
        }
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
         Post? existingPost = posts.SingleOrDefault(p => p.PostId == post.PostId);

        if (existingPost is null) throw new InvalidOperationException($"Post with ID {post.PostId} not found");

        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string postId)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.PostId == postId);

        if (postToRemove is null) throw new InvalidOperationException($"Post with ID {postId} not found");

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(string postId)
    {
        var post = posts.FirstOrDefault(p => p.PostId == postId);
        if (post is null) throw new InvalidOperationException($"Comment with ID '{postId}' not found");
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
    
    public Task LikePostAsync(string id)
    {
        var postToLike = posts.SingleOrDefault(c => c.PostId == id);
        if (postToLike is null) throw new InvalidOperationException($"Post with ID '{id}' not found");

        postToLike.Likes++;

        return Task.CompletedTask;
    }
    
    public Task RemoveLikePostAsync(string id)
    {
        var postToLike = posts.SingleOrDefault(c => c.PostId == id);
        if (postToLike is null) throw new InvalidOperationException($"Post with ID '{id}' not found");

        if (postToLike.Likes > 0)
        {
            postToLike.Likes--;
        }

        return Task.CompletedTask;
    }
}