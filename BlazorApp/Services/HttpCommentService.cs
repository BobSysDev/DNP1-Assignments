using System.Text.Json;
using DTOs;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private HttpClient client;
    
    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<CommentDTO> CreateCommentAsync(CreateCommentDTO dto)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("Comment", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        var responseDto = JsonSerializer.Deserialize<CommentDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (responseDto is not null)
        {
            return responseDto;
        }

        throw new Exception("No data to deserialize.");
    }

    public async Task<CommentDTO> GetSingleCommentByIdAsync(string id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"Comment/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        var responseDto = JsonSerializer.Deserialize<CommentDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (responseDto is not null)
        {
            return responseDto;
        }

        throw new Exception("No data to deserialize.");
    }

    public async Task<CommentDTO> UpdateCommentAsync(string id, UpdateCommentDTO dto)
    {
        HttpResponseMessage httpResponse = await client.PatchAsJsonAsync($"Comment/{id}", dto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        var responseDto = JsonSerializer.Deserialize<CommentDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (responseDto is not null)
        {
            return responseDto;
        }

        throw new Exception("No data to deserialize.");
    }

    public async Task DeleteCommentAsync(string id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"Comment/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task LikeCommentAsync(string id)
    {
        HttpResponseMessage httpResponse = await client.PostAsync($"Comment/{id}/like", null);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task DislikeCommentAsync(string id)
    {
        HttpResponseMessage httpResponse = await client.PostAsync($"Comment/{id}/dislike", null);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<List<CommentDTO>> GetCommentsByPostIdAsync(string id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"Post/{id}/Comments");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        var responseDto = JsonSerializer.Deserialize<List<CommentDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (responseDto is not null)
        {
            return responseDto;
        }

        throw new Exception("No data to deserialize.");
    }

    public async Task<List<SimpleCommentDTO>> GetCommentsByUserIdAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"User/{id}/Comments");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        var responseDto = JsonSerializer.Deserialize<List<SimpleCommentDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (responseDto is not null)
        {
            return responseDto;
        }

        throw new Exception("No data to deserialize.");
    }
}