﻿using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageForumUsers;

public class DeleteUser
{
    private readonly IUserRepository _userRepository;

    public DeleteUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task DeleteForumUser()
    {
        var users = _userRepository.GetMany().ToList();
        
        if (!users.Any())
        {
            Console.WriteLine("No users available.");
            return;
        }
        
        Console.WriteLine("Users to delete:");
        foreach (var user in users)
        {
            Console.WriteLine($"User ID: {user.Id}, Username: {user.Username}");
        }
        
        Console.WriteLine("Enter the user ID to delete:");
        var userIdInput = Console.ReadLine();
        
        var userToDelete = users.FirstOrDefault(u => u.Id.Equals(userIdInput) );
        if (userToDelete == null)
        {
            Console.WriteLine($"User with ID {userIdInput} not found.");
            return;
        }
        
        Console.WriteLine($"Are you sure you want to delete user '{userToDelete.Username}'? (yes/no)");
        var confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "yes")
        {
            await _userRepository.DeleteAsync(userIdInput);
            Console.WriteLine($"User {userIdInput} deleted successfully!");
        }
        else
        {
            Console.WriteLine("User not deleted.");
        }
    }
}