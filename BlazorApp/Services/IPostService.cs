using DTOs;
using Entities;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDTO> GetPostByIdAsync(string postId);
    Task<IEnumerable<PostDTO>> GetAllPostsAsync();
    Task AddPostAsync(PostDTO postDto);
    Task<PostDTO> UpdatePostAsync(string postId, CreatePostDTO postDto);
    Task DeletePostAsync(string postId);
}