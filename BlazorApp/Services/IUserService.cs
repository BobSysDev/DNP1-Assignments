using DTOs;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDTO> AddUserAsync(CreateUserDTO request);
    Task<List<PublicUserDTO>> GetAll();
    Task<PublicUserDTO> GetUserById(int id);
    Task<PublicUserDTO> UpdateUser(UserDTO update);
    Task DeleteUser(int id);

}
