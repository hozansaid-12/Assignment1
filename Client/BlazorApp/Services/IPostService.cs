using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDto> GetSinglePostAsync(int id);
    Task<IEnumerable<PostDto>> GetManyPostsAsync(
        string? titleContains = null, int? userId = null);
    Task UpdatePostAsync(int id, CreatePostDto request);
    Task DeletePostAsync(int id);
}