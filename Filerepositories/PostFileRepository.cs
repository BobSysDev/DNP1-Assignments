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
        var postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        
        if (posts == null)
        {
            throw new InvalidDataException("Deserializing posts failed");
        }

        if (posts.Any())
        {
            var resultPosts = posts.Max(p => Int32.Parse(p.PostId.Substring(1, p.PostId.Length - 1))) + 1;
            post.PostId = "P" + resultPosts;
        }
        else
        {
            post.PostId = "P1";
        }
        
        posts.Add(post);
        postAsJson = JsonSerializer.Serialize(posts, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(filePath, postAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        
        if (posts == null)
        {
            throw new InvalidDataException("Deserializing posts failed");
        }
        
        var existingPost = posts.FirstOrDefault(p => p.PostId == post.PostId);
        if (existingPost != null)
        {
            posts.Remove(existingPost);
            posts.Add(post);
            postAsJson = JsonSerializer.Serialize(posts, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(filePath, postAsJson);
        }
    }

    public async Task DeleteAsync(string postId)
    {
        var postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        
        if (posts == null)
        {
            throw new InvalidDataException("Deserializing posts failed");
        }
        
        var existingPost = posts.FirstOrDefault(p => p.PostId == postId);
        if (existingPost != null)
        {
            posts.Remove(existingPost);
            postAsJson = JsonSerializer.Serialize(posts, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        await File.WriteAllTextAsync(filePath, postAsJson);
    }

    public async Task<Post> GetSingleAsync(string postId)
    {
        var postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        return posts.FirstOrDefault(p => p.PostId == postId);
    }

    public IQueryable<Post> GetMany()
    {
        var postAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        return posts.AsQueryable();
    }

    public async Task LikePostAsync(string id)
    {
        var postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        var post = posts.FirstOrDefault(p => p.PostId == id);
        if (posts != null)
        {
            post.Likes++;
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(posts));
        }
    }

    public async Task RemoveLikePostAsync(string id)
    {
        var postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        var post = posts.FirstOrDefault(p => p.PostId == id);
        if (posts != null)
        {
            post.Likes--;
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(posts));
        }
        
        
    }
}