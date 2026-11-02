using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<CommentDto> GetSingleCommentAsync(int id);
    Task<IEnumerable<CommentDto>> GetManyCommentsAsync(
        int? userId = null, int? postId = null);
    Task UpdateCommentAsync(int id, CreateCommentDto request);
    Task DeleteCommentAsync(int id);
}