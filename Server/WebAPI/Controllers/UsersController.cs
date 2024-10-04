using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
    
    
    [HttpPost] public async Task<ActionResult<User>> AddUser([FromBody] UserDTO request) { await VerifyUserNameIsAvailableAsync(request.Username, request.Id);

        User user = new(request.Username, request.Password, request.Id); User created = await userRepo.AddAsync(user);

        UserDTO dto = new()
        {

            Id = created.Id,

            Username = created.Username

        };
        return Created($"/Users/{dto.Id}", created);}

    
    private async Task VerifyUserNameIsAvailableAsync(string newUsername, int userId)
    {
        var currentUser = await userRepo.GetSingleAsync(userId);
    
        if (currentUser == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (currentUser.Username == newUsername)
        {
            return;
        }

        var existingUser = await userRepo.GetByUsernameAsync(newUsername);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username is already taken.");
        }
    }

    
    
    
}
