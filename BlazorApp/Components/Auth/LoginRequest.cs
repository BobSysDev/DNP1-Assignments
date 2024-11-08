namespace BlazorApp.Components.Auth;

public class LoginRequest
{
    public static string Username { get; set; }
    public static string Password { get; set; }

    public LoginRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}