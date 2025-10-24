using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.DTOs;
using UserApi.Application.Interfaces;
using UserApi.Domain.Exceptions;

namespace UserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService, IValidator<CreateUserDto> validator, ILogger<UsersController> logger) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
        try
        {
            var createdUser = await userService.CreateUserAsync(dto);
            logger.LogInformation("User created with ID: {UserId}", createdUser.Id);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (UserAlreadyExistsException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (UserNoTFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
