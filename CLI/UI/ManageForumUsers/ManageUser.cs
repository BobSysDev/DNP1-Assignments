using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageForumUsers;

public class ManageUser
{
    private readonly IUserRepository _userRepository;

    public ManageUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task ManageForumUser()
    {
        Console.WriteLine("1. Create a user");
        Console.WriteLine("2. Delete a user");
        Console.WriteLine("3. List all users");
        var choice = Console.ReadLine();
    
        switch (choice)
        {
            case "1":
                var createUser = new CreateUser(_userRepository);
                await createUser.CreateForumUser();
                break;
            case "2":
                var deleteUser = new DeleteUser(_userRepository);
                await deleteUser.DeleteForumUser();
                break;
            case "3" :
                var updateUser = new UpdateUser(_userRepository);
                await updateUser.UpdateForumUser();
                break;
            case "4":
                var listUser = new ListUser(_userRepository);
                listUser.DisplayForumUsers();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}