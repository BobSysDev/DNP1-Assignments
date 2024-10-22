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


    [HttpPost]
    public async Task<ActionResult<User>> AddUser([FromBody] UserDTO request)
    {
        //await VerifyUserNameIsAvailableAsync(request.Username, request.Id);

        User user = new(request.Username, request.Password, request.Id);
        User created = await userRepo.AddAsync(user);

        UserDTO dto = new()
        {

            Id = created.Id,

            Username = created.Username

        };
        return Created($"/Users/{dto.Id}", created);
    }


    private async Task VerifyUserNameIsAvailableAsync(string newUsername, int userId)
    {
        // var currentUser = await userRepo.GetSingleAsync(userId);
        //
        // if (currentUser == null)
        // {
        //     throw new InvalidOperationException("User not found.");
        // }
        //
        // if (currentUser.Username == newUsername)
        // {
        //     return;
        // }
        //
        // var existingUser = await userRepo.GetByUsernameAsync(newUsername);
        // if (existingUser != null)
        // {
        //     throw new InvalidOperationException("Username is already taken.");
        // }
    }

    [HttpPatch]
    public async Task<ActionResult<User>> UpdateUser([FromBody] UserDTO request)
    {
        User user = new(request.Username, request.Password, request.Id);
        userRepo.UpdateAsync(user);
        User updated = await userRepo.GetSingleAsync(user.Id);
        UserDTO dto = new()
        {
            Id = updated.Id,
            Username = updated.Username
        };
        return Accepted($"/Users/{dto.Id}", updated);
    }

    [HttpGet]
    public async Task<ActionResult<User>> GetSingle([FromBody] UserDTO request)
    {
        User gotUser = await userRepo.GetSingleAsync(request.Id);
        if (gotUser is null)
        {
            throw new InvalidOperationException("User does not exist.");
        }

        UserDTO dto = new()
        {
            Id = gotUser.Id,
            Username = gotUser.Username
        };
        return Accepted($"/Users/{dto.Id}", gotUser);
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetMany([FromBody] string username)
    {
        List<User> users = new List<User>();
        users.AddRange(userRepo.GetMany());
        IEnumerable<User> usersFound = users.Where(user => user.Username.Contains(username));
        
        
    }
}
