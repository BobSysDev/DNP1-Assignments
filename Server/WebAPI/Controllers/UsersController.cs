using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("Users")]  
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    // Other methods...

    [HttpGet]  // This now aligns with the "/Users" route
    public async Task<ActionResult<List<PublicUserDTO>>> GetAll()
    {
        try
        {
            List<User> users = await userRepo.GetAllAsync();
            List<PublicUserDTO> dtos = users.Select(u => new PublicUserDTO
            {
                Id = u.Id,
                Username = u.Username
            }).ToList();

            return Ok(dtos); // Returning OK with dtos
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetAll: {e.Message}");
            return Problem(e.Message);
        }
    }
}