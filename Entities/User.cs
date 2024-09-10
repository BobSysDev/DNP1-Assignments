namespace Entities;

public class User
{
    public User(string username, string password, int id)
    {
        Username = username;
        Password = password;
        Id = id;
    }

    public string Username { get; set; }
    public string Password { get; set; }
    public int Id { get; set; }
}