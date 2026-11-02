using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("users", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<UserDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<UserDto> GetSingleUserAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"users/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<UserDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<IEnumerable<UserDto>> GetManyUsersAsync(string? usernameContains = null)
    {
        string endpoint = string.IsNullOrWhiteSpace(usernameContains)
            ? "users"
            : $"users?usernameContains={Uri.EscapeDataString(usernameContains)}";

        HttpResponseMessage response = await client.GetAsync(endpoint);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<List<UserDto>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UserDto>();
    }

    public async Task UpdateUserAsync(int id, CreateUserDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"users/{id}", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
    }

    public async Task DeleteUserAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"users/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
    }
}