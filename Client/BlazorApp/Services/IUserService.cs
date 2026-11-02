using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<UserDto> GetSingleUserAsync(int id);
    Task<IEnumerable<UserDto>> GetManyUsersAsync(string? usernameContains = null);
    Task UpdateUserAsync(int id, CreateUserDto request);
    Task DeleteUserAsync(int id);
}