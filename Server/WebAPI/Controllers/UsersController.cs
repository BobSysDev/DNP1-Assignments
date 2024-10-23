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
    public async Task<ActionResult<User>> AddUser([FromBody] CreateUserDTO request)
    {
        await VerifyUserNameIsAvailableAsync(request.Username);

        User user = new(request.Username, request.Password, -1);
        User created = await userRepo.AddAsync(user);

        UserDTO dto = new()
        {

            Id = created.Id,

            Username = created.Username

        };
        return Created($"/Users/{dto.Id}", created);
    }


    private async Task VerifyUserNameIsAvailableAsync(string newUsername)
    {
        // var currentUser = await userRepo.GetSingleAsync(userId);
        //
        // if (currentUser == null)
        // {
        //     throw new InvalidOperationException("User not found.");
        // }
        
        // if (currentUser.Username == newUsername)
        // {
        //     return;
        // }
        
        var existingUser = await userRepo.GetByUsernameAsync(newUsername);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username is already taken.");
        }
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

    [HttpGet("/User/{id}")]
    public async Task<ActionResult<User>> GetSingle([FromRoute] int id)
    {
        User gotUser = await userRepo.GetByIdAsync(id);
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

    [HttpGet ("/Users/{username}")]

    public async Task<ActionResult<List<User>>> GetMany([FromRoute] string username)
    {
        List<User> users = new List<User>();
        users.AddRange(userRepo.GetMany());
        List<User> usersFound = users.Where(user => user.Username.Contains(username)).ToList();
        
        List<UserDTO> dtos = new();
        for (int i = 0; i < usersFound.Count; i++)
        {
            UserDTO dto = new()
            {
                Id = usersFound.ElementAt(i).Id,
                Username = usersFound.ElementAt(i).Username
            };
            dtos.Add(dto);
        }
        return Accepted($"/Users/{dtos}", usersFound);
    }

    [HttpDelete]public async Task<ActionResult<User>> Delete([FromBody] DeleteUserDTO request)
    {
        User userToDelete = await userRepo.GetByIdAsync(request.Id);
        if (userToDelete.Password == request.Password)
        {
            userRepo.DeleteAsync(request.Id);
            return Ok();
        }
        return Unauthorized("Wrong password.");
    }
}
