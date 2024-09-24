using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace Filerepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }
    
    
    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        String maxId = posts.Count > 0 ? posts.Max(p => p.PostId) + 1 : 1;
        post.PostId = maxId;
    }

    public Task UpdateAsync(Post post)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string postId)
    {
        throw new NotImplementedException();
    }

    public Task<Post> GetSingleAsync(string postId)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Post> GetMany()
    {
        throw new NotImplementedException();
    }

    public Task LikePostAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task RemoveLikePostAsync(string id)
    {
        throw new NotImplementedException();
    }
}