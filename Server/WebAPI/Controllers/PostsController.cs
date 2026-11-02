
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;

    public PostsController(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> AddPost(
        [FromBody] Post request)
    {
        Post post = new()
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };

        Post created = await postRepo.AddAsync(post);

        return Created($"/posts/{created.Id}", created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(
        int id, [FromBody] Post request)
    {
        Post post = new()
        {
            Id = id,
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };

        await postRepo.UpdateAsync(post);

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>> GetSinglePost(int id)
    {
        Post post = await postRepo.GetSingleAsync(id);

        return Ok(post);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetManyPosts(
        [FromQuery] string? titleContains,
        [FromQuery] int? userId)
    {
        IQueryable<Post> posts = postRepo.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            posts = posts.Where(post =>
                post.Title.Contains(titleContains));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(post =>
                post.UserId == userId.Value);
        }

        return Ok(posts.ToList());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        await postRepo.DeleteAsync(id);

        return NoContent();
    }
}
