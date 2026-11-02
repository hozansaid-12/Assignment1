using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("comments", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<CommentDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<CommentDto> GetSingleCommentAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"comments/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<CommentDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<IEnumerable<CommentDto>> GetManyCommentsAsync(
        int? userId = null, int? postId = null)
    {
        List<string> parameters = new();

        if (userId.HasValue)
            parameters.Add($"userId={userId.Value}");

        if (postId.HasValue)
            parameters.Add($"postId={postId.Value}");

        string endpoint = parameters.Count == 0
            ? "comments"
            : $"comments?{string.Join("&", parameters)}";

        HttpResponseMessage response = await client.GetAsync(endpoint);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<List<CommentDto>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? new List<CommentDto>();
    }

    public async Task UpdateCommentAsync(int id, CreateCommentDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"comments/{id}", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"comments/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
    }
}