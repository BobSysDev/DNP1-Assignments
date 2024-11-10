using DTOs;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDTO> AddUserAsync(CreateUserDTO request);
    Task<List<UserDTO>> GetAll();
    Task<PublicUserDTO> GetUserById(int id);
}
