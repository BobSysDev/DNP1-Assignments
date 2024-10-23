using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace Filerepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string _filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        var commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        
        if (comments is null)
        {
            throw new InvalidDataException("Deserializing comment list returned NULL");
        }
        
        if (comments.Any())
        {
            var result = comments.Max(c => Int32.Parse(c.Id.Substring(1, c.Id.Length - 1))) + 1;
            comment.Id = "C" + result;
        }
        else
        {
            comment.Id = "C1";
        }
        
        comments.Add(comment);
        
        commentsAsJson = JsonSerializer.Serialize(comments, options: new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
        
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        
        if (comments is null)
        {
            throw new InvalidDataException("Deserializing comment list returned NULL");
        }
        
        var existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null) throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");

        comments.Remove(existingComment);
        comments.Add(comment);
        
        commentsAsJson = JsonSerializer.Serialize(comments, options: new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
    }

    public async Task DeleteCascadeAsync(string id)
    {
        var commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        
        if (comments is null)
        {
            throw new InvalidDataException("Deserializing comment list returned NULL");
        }
        
        await DeleteCascade(id, comments);
    }
    
    public async Task DeleteAsync(string id)
    {
        var commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        
        if (comments is null)
        {
            throw new InvalidDataException("Deserializing comment list returned NULL");
        }
        
        var commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null) throw new InvalidOperationException($"Comment with ID '{id}' not found");

        comments.Remove(commentToRemove); 
        
        commentsAsJson = JsonSerializer.Serialize(comments, options: new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
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
    
    public async Task<Comment> GetSingleAsync(string id)
    {
        var commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        
        if (comments is null)
        {
            throw new InvalidDataException("Deserializing comment list returned NULL");
        }
        
        var commentToRetrieve = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRetrieve is null) throw new InvalidOperationException($"Comment with ID '{id}' not found");
        
        return commentToRetrieve;
    }
    
    public IQueryable<Comment> GetManyAsync()
    {
        var commentsAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        
        if (comments is null)
        {
            throw new InvalidDataException("Deserializing comment list returned NULL");
        }
        
        return comments.AsQueryable();
    }
    
    public async Task LikeCommentAsync(string id)
    {
        var commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        
        if (comments is null)
        {
            throw new InvalidDataException("Deserializing comment list returned NULL");
        }
        
        var commentToLike = comments.SingleOrDefault(c => c.Id == id);
        if (commentToLike is null) throw new InvalidOperationException($"Comment with ID '{id}' not found");

        commentToLike.Likes++;

        commentsAsJson = JsonSerializer.Serialize(comments, options: new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
    }
    
    public async Task RemoveLikeCommentAsync(string id)
    {
        var commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        
        if (comments is null)
        {
            throw new InvalidDataException("Deserializing comment list returned NULL");
        }
        
        var commentToLike = comments.SingleOrDefault(c => c.Id == id);
        if (commentToLike is null) throw new InvalidOperationException($"Comment with ID '{id}' not found");

        if (commentToLike.Likes > 0)
        {
            commentToLike.Likes--;
        }

        commentsAsJson = JsonSerializer.Serialize(comments, options: new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
    }
}