using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("posts", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<PostDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<PostDto> GetSinglePostAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"posts/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<PostDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<IEnumerable<PostDto>> GetManyPostsAsync(
        string? titleContains = null, int? userId = null)
    {
        List<string> parameters = new();

        if (!string.IsNullOrWhiteSpace(titleContains))
            parameters.Add($"titleContains={Uri.EscapeDataString(titleContains)}");

        if (userId.HasValue)
            parameters.Add($"userId={userId.Value}");

        string endpoint = parameters.Count == 0
            ? "posts"
            : $"posts?{string.Join("&", parameters)}";

        HttpResponseMessage response = await client.GetAsync(endpoint);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<List<PostDto>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? new List<PostDto>();
    }

    public async Task UpdatePostAsync(int id, CreatePostDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"posts/{id}", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"posts/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
    }
}