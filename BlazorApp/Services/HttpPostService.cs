
using System.Text.Json;
using DTOs;
using Entities;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient _httpClient;

    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PostDTO> GetPostByIdAsync(string postId)
    {
        return await _httpClient.GetFromJsonAsync<PostDTO>($"posts/{postId}");
    }

    public async Task<IEnumerable<PostDTO>> GetAllPostsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<PostDTO>>("posts");
    }

    public async Task AddPostAsync(PostDTO postDto)
    {
        await _httpClient.PostAsJsonAsync("posts", postDto);
    }

    public async Task<PostDTO> UpdatePostAsync(string postId, CreatePostDTO postDto)
    {
        HttpResponseMessage response = await _httpClient.PatchAsJsonAsync($"posts/{postId}", postDto);
        PostDTO responseDTO = JsonSerializer.Deserialize<PostDTO>(await response.Content.ReadAsStringAsync());
        return responseDTO;
    }

    public async Task DeletePostAsync(string postId)
    {
        await _httpClient.DeleteAsync($"posts/{postId}");
    }
}