using DTOs;
using Entities;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDTO> GetPostByIdAsync(string postId);
    Task<IEnumerable<PostDTO>> GetAllPostsAsync();
    Task AddPostAsync(PostDTO postDto);
    Task UpdatePostAsync(PostDTO postDto);
    Task DeletePostAsync(string postId);
}