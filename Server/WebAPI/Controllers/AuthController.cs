using DTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private IUserRepository _userRepository { get; set; }
    
    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost("authenticate")]
    public async Task<IResult> Authenticate([FromBody] AuthUserDTO request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return Results.BadRequest("Username or password cannot be empty.");
        }

        try
        {
            var userFromRepository = await _userRepository.GetByUsernameAsync(request.Username);
            var dtoToReturn = new PublicUserDTO
            {
                Username = userFromRepository.Username,
                Id = userFromRepository.Id
            };
            if (userFromRepository.Password == request.Password)
            {
                return Results.Ok(dtoToReturn);
            }
            else
            {
                return Results.Unauthorized();
            }
        }
        catch (Exception e)
        {
            return Results.Problem("User not found (or something else is wrong, idk, figure it out)");
        }
    }
}