
using Entities;

namespace BlazorApp.Components.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient _httpClient;

    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Post> GetPostByIdAsync(string postId)
    {
        return await _httpClient.GetFromJsonAsync<Post>($"posts/{postId}");
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Post>>("posts");
    }

    public async Task AddPostAsync(Post post)
    {
        await _httpClient.PostAsJsonAsync("posts", post);
    }

    public async Task UpdatePostAsync(Post post)
    {
        await _httpClient.PutAsJsonAsync($"posts/{post.PostId}", post);
    }

    public async Task DeletePostAsync(string postId)
    {
        await _httpClient.DeleteAsync($"posts/{postId}");
    }
}