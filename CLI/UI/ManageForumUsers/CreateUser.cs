using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageForumUsers;

public class CreateUser
{
    private readonly IUserRepository _userRepository;

    public CreateUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task CreateForumUser()
    {
        Console.WriteLine("Enter username:");
        var username = Console.ReadLine();
        
        Console.WriteLine("Enter password:");
        var password = Console.ReadLine();
        
        var newUser = new User (username, password,1 );
        await _userRepository.AddAsync(newUser);
        Console.WriteLine("User created successfully");
    }
}