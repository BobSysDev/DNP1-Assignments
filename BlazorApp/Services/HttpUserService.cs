using System.Net.Http.Json;
using System.Text.Json;
using BlazorApp.Components.Pages;
using DTOs;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDTO> AddUserAsync(CreateUserDTO request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("/Users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<PublicUserDTO>> GetAll()
    {
        var response = await client.GetFromJsonAsync<List<PublicUserDTO>>("/Users");
        return response ?? new List<PublicUserDTO>();
    }

    public async Task<PublicUserDTO> GetUserById(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"/User/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PublicUserDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<PublicUserDTO> UpdateUser(UserDTO update)
    {
        HttpResponseMessage response = await client.PatchAsJsonAsync("/Users/", update);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
        return JsonSerializer.Deserialize<PublicUserDTO>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task DeleteUser(int id)
    {
        await client.DeleteAsync($"/Users/{id}");
    }
}