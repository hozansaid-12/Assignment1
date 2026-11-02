
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser(
        [FromBody] CreateUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.Username);

        User user = new()
        {
            Username = request.Username,
            Password = request.Password
        };

        User created = await userRepo.AddAsync(user);

        UserDto dto = new()
        {
            Id = created.Id,
            Username = created.Username
        };

        return Created($"/users/{dto.Id}", dto);
    }

   
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(
        int id, [FromBody] CreateUserDto request)
    {
        User user = new()
        {
            Id = id,
            Username = request.Username,
            Password = request.Password
        };

        await userRepo.UpdateAsync(user);

        return NoContent();
    }

    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingleUser(int id)
    {
        User user = await userRepo.GetSingleAsync(id);

        UserDto dto = new()
        {
            Id = user.Id,
            Username = user.Username
        };

        return Ok(dto);
    }

    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetManyUsers(
        [FromQuery] string? usernameContains)
    {
        IQueryable<User> users = userRepo.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(usernameContains))
        {
            users = users.Where(user =>
                user.Username.Contains(usernameContains));
        }

        IEnumerable<UserDto> dtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username
        }).ToList();

        return Ok(dtos);
    }

    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await userRepo.DeleteAsync(id);

        return NoContent();
    }

 
    private Task VerifyUserNameIsAvailableAsync(string username)
    {
        bool usernameExists = userRepo.GetManyAsync()
            .Any(user => user.Username == username);

        if (usernameExists)
        {
            throw new InvalidOperationException(
                $"Username '{username}' is already taken");
        }

        return Task.CompletedTask;
    }
}
