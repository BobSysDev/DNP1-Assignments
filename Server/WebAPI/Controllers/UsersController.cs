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
        if(request.Username == null || request.Username.Equals(""))
        {
            return BadRequest("Username required.");
        }
        
        if(request.Password == null || request.Password.Equals(""))
        {
            return BadRequest("Password required.");
        }
        
        if(request.Username.Equals("string")|| request.Password.Equals("string"))
        {
            return BadRequest("Invalid input.");
        }

        try
        {
            Task<Boolean> ver = VerifyUserNameIsAvailableAsync(request.Username);
            if (ver.Result == false)
            {
               return BadRequest($"Username ({request.Username}) is not available.");
            }
            User user = new(request.Username, request.Password, -1);
            User created = await userRepo.AddAsync(user);

            PublicUserDTO dto = new()
            {

                Id = created.Id,

                Username = created.Username

            };
            return Created($"/Users/{dto.Id}", dto);
        }
        catch (InvalidOperationException e)
        {
            return Problem(e.Message); 
        }
        catch (InvalidDataException e)
        {
            return Problem(e.Message); 
        }
        catch (Exception e)
        {
            return Problem(e.Message); 
        }
    }


    private async Task<Boolean> VerifyUserNameIsAvailableAsync(string newUsername)
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
        
        try
        {
            User existingUser = await userRepo.GetByUsernameAsync(newUsername);
            if (existingUser != null)
            {
                return false;
            }
        }
        catch (InvalidOperationException e)
        {
            return true;
        }
        catch (InvalidDataException e)
        {
            return false;
        }
        
        return false;
    }

    [HttpPatch]
    public async Task<ActionResult<User>> UpdateUser([FromBody] UserDTO request)
    {
        if(request.Username == null || request.Username.Equals(""))
        {
            return BadRequest("Username required.");
        }
        
        if(request.Password == null || request.Password.Equals(""))
        {
            return BadRequest("Password required.");
        }
        
        if(request.Username.Equals("string")|| request.Password.Equals("string"))
        {
            return BadRequest("Invalid input.");
        }

        try
        {
            User user = new(request.Username, request.Password, request.Id);
            await userRepo.UpdateAsync(user);
            User updated = await userRepo.GetSingleAsync(user.Id);
            UserDTO dto = new()
            {
                Id = updated.Id,
                Username = updated.Username
            };
            return Accepted($"/Users/{dto.Id}", updated);
        }
        catch (InvalidDataException e)
        {
            return Problem(e.Message); 
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message); 
        }
        
    }

    [HttpGet("/User/{id}")]
    public async Task<ActionResult<User>> GetSingle([FromRoute] int id)
    {
        if(id==null)
        {
            return BadRequest("Id required.");
        }

        try
        {
            User gotUser = await userRepo.GetByIdAsync(id);

            PublicUserDTO dto = new()
            {
                Id = gotUser.Id,
                Username = gotUser.Username
            };
            return Accepted($"/Users/{dto.Id}", dto);
        }
        catch (InvalidDataException e)
        {
            return Problem(e.Message); 
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message); 
        }
        
    }

    [HttpGet ("/Users/{username}")]

    public async Task<ActionResult<List<User>>> GetMany([FromRoute] string username)
    {
        if(username==null)
        {
            return BadRequest("Input required.");
        }

        try
        {
            List<User> users = new List<User>();
            users.AddRange(userRepo.GetMany());
            List<User> usersFound = users.Where(user => user.Username.Contains(username)).ToList();
        
            List<PublicUserDTO> dtos = new();
            for (int i = 0; i < usersFound.Count; i++)
            {
                PublicUserDTO dto = new()
                {
                    Id = usersFound.ElementAt(i).Id,
                    Username = usersFound.ElementAt(i).Username
                };
                dtos.Add(dto);
            }
            return Accepted($"/Users/{dtos}", dtos);
        }
        catch (Exception e)
        {
            return Problem(e.Message); 
        }
        
    }

    [HttpDelete]public async Task<ActionResult<User>> Delete([FromBody] DeleteUserDTO request)
    {
        if (request.Id == null || request.Id == 0) 
        {
            return BadRequest("ID required.");
        }
        if(request.Password==null || request.Password.Equals(""))
        {
            return BadRequest("Password required.");
        }

        try
        {
            User userToDelete = await userRepo.GetByIdAsync(request.Id);
            if (userToDelete.Password == request.Password)
            {
                userRepo.DeleteAsync(request.Id);
                return Ok();
            }
            return Unauthorized("Wrong password.");
        }
        catch (InvalidDataException e)
        {
            return Problem(e.Message); 
        }
        catch (InvalidOperationException e)
        {
            return Problem(e.Message); 
        }
        
    }
}
