using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = new List<Comment>();

    public Task<Comment> AddAsync(Comment comment)
    {
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
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        var existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null) throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");

        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        var commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null) throw new InvalidOperationException($"Comment with ID '{id}' not found");

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(string id)
    {
        var commentToRetrieve = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRetrieve is null) throw new InvalidOperationException($"Comment with ID '{id}' not found");

        return Task.FromResult(commentToRetrieve);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return comments.AsQueryable();
    }

    public Task LikeCommentAsync(string id)
    {
        var commentToLike = comments.SingleOrDefault(c => c.Id == id);
        if (commentToLike is null) throw new InvalidOperationException($"Comment with ID '{id}' not found");

        commentToLike.Likes++;

        return Task.CompletedTask;
    }
    
    public Task RemoveLikeCommentAsync(string id)
    {
        var commentToLike = comments.SingleOrDefault(c => c.Id == id);
        if (commentToLike is null) throw new InvalidOperationException($"Comment with ID '{id}' not found");

        if (commentToLike.Likes > 0)
        {
            commentToLike.Likes--;
        }

        return Task.CompletedTask;
    }
}