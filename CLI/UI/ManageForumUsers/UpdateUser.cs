using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageForumUsers;

public class UpdateUser
{
    private IUserRepository _userRepository;
    
    public UpdateUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task UpdateForumUser(int userId)
    {
        var updatedUser = await _userRepository.GetSingleAsync(userId);
        while (true)
        {
            Console.WriteLine($"Updating user [ {userId} ]. ");
            Console.WriteLine("Write the new username here, or write \"<C>\" to cancel: ");
            var content = Console.ReadLine();
            if (content is not null)
            {
                if (content == "<C>")
                {
                    return;
                }

                updatedUser.Username = content;
                await _userRepository.UpdateAsync(updatedUser);
                break;
            }
            Console.WriteLine("Try again...");
        }
        
        while (true)
        {
            Console.WriteLine($"Updating user [ {userId} ]. ");
            Console.WriteLine("Write the new password here, or write \"<C>\" to cancel: ");
            var content = Console.ReadLine();
            if (content is not null)
            {
                if (content == "<C>")
                {
                    return;
                }

                updatedUser.Password = content;
                await _userRepository.UpdateAsync(updatedUser);
                break;
            }
            Console.WriteLine("Try again...");
        }
    }
}