
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> AddComment(
        [FromBody] Comment request)
    {
        Comment comment = new()
        {
            Body = request.Body,
            UserId = request.UserId,
            PostId = request.PostId
        };

        Comment created = await commentRepo.AddAsync(comment);

        return Created($"/comments/{created.Id}", created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateComment(
        int id, [FromBody] Comment request)
    {
        Comment comment = new()
        {
            Id = id,
            Body = request.Body,
            UserId = request.UserId,
            PostId = request.PostId
        };

        await commentRepo.UpdateAsync(comment);

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Comment>> GetSingleComment(int id)
    {
        Comment comment = await commentRepo.GetSingleAsync(id);

        return Ok(comment);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Comment>> GetManyComments(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        IQueryable<Comment> comments = commentRepo.GetManyAsync();

        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        return Ok(comments.ToList());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await commentRepo.DeleteAsync(id);

        return NoContent();
    }
}
