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

    public async Task<List<UserDTO>> GetAll()
    {
        var response = await client.GetFromJsonAsync<List<UserDTO>>("/Users");
        return response ?? new List<UserDTO>();
    }

}