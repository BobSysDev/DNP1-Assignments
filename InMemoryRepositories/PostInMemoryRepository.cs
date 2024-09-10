﻿using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = new List<Post>();

    public Task<Post> AddAsync(Post post)
    {
        post.PostId = posts.Any() ? posts.Max(p => p.PostId) + 1 : 1;
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

    public Task DeleteAsync(int postId)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.PostId == postId);

        if (postToRemove is null) throw new InvalidOperationException($"Post with ID {postId} not found");

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int postId)
    {
        var post = posts.FirstOrDefault(p => p.PostId == postId);
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}