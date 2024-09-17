using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageForumUsers;

public class ListUser
{
    private readonly IUserRepository _userRepository;

    public ListUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public void DisplayForumUsers()
    {
      
        var users = _userRepository.GetMany().ToList();
        
        if (!users.Any())
        {
            Console.WriteLine("No users available.");
            return;
        }
        
        Console.WriteLine("List of users:");
        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.Username}, Password: {user.Password}, ID: {user.Id}");
        }
    }
}