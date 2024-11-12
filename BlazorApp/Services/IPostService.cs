﻿using Entities;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<Post> GetPostByIdAsync(string postId);
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task AddPostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(string postId);
}