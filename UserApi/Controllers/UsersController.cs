using Microsoft.AspNetCore.Mvc;
using UserApi.Service;

namespace UserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        try
        {
            var createdUser = await userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
