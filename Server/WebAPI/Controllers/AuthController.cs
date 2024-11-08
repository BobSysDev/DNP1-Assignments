using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    
    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("authenticate")]

    public async Task<ActionResult<PublicUserDTO>> Authenticate([FromBody] LoginDTO request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return Unauthorized("Username and password are required");
        }

        try
        {
            User user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            if (user.Password != request.Password)
            {
                return Unauthorized("Invalid password");
            }
            
            PublicUserDTO publicUser = new PublicUserDTO
            {
                Id = user.Id,
                Username = user.Username
            };

            return Ok(publicUser);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}