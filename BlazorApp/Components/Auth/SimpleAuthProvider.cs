using System.Security.Claims;
using System.Text.Json;
using DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.Data;

namespace BlazorApp.Components.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private ClaimsPrincipal currentCaClaimsPrincipal;

    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(currentCaClaimsPrincipal ?? new());
    }

    public async Task Login(string username, string password)
    {
        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync("/Auth/authenticate", new AuthUserDTO{Password = password, Username = username});
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
        PublicUserDTO? publicUserDto = JsonSerializer.Deserialize<PublicUserDTO>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (publicUserDto is null)
        {
            throw new Exception("DTO returned was null");
        }

        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, publicUserDto.Username));
        claims.Add(new Claim("Id", publicUserDto.Id.ToString()));

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        currentCaClaimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentCaClaimsPrincipal)));
    }

    public void Logout()
    {
        currentCaClaimsPrincipal = new();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentCaClaimsPrincipal)));
    }
}