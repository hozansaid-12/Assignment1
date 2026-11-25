
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<PostDto>> AddPost(
        [FromBody] CreatePostDto request)
    {
        Post post = new(request.Title, request.Body, request.UserId);

        Post created = await postRepo.AddAsync(post);

        PostDto response = ToDto(created);

        return Created($"/posts/{created.Id}", response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(
        int id, [FromBody] CreatePostDto request)
    {
        Post post = new(request.Title, request.Body, request.UserId)
        {
            Id = id
        };

        await postRepo.UpdateAsync(post);

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSinglePost(int id)
    {
        Post post = await postRepo.GetSingleAsync(id);

        return Ok(ToDto(post));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetManyPosts(
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

        List<PostDto> result = await posts
            .Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        await postRepo.DeleteAsync(id);

        return NoContent();
    }

    private static PostDto ToDto(Post post)
    {
        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId
        };
    }
}

