using API.DTOs;
using API.DTOs.Responses;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController: ControllerBase
{
    private readonly IUserService _userService;
    
    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var authResponse = await _userService.LoginAsync(request.Email, request.Password);
        if (!authResponse.Success)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        }

        // TODO: Add domain and path later for extra security 'Lenny Face'
        // TODO: Change cookie name afterwards
        Response.Cookies.Append("refreshToken", authResponse.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
        });; 
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        var authResponse = await _userService.RegisterAsync(request.Email, request.Password);
        if (!authResponse.Success)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        }

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
        });
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        // Move token name to constants or somewhere close to that
        KeyValuePair<string, string>? refreshToken = Request.Cookies.FirstOrDefault(x => x.Key == "refreshToken");
        if (refreshToken == null)
            return BadRequest(new AuthFailedResponse { Errors = new string[] { "Refresh token is not present" } });

        var authResponse = await _userService.RefreshAsync(refreshToken.Value.Value.ToString());
        if (!authResponse.Success)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        }

        Response.Cookies.Append("refreshToken", authResponse.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
        });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] Guid id) 
    {
        var deleteResponse = await _userService.DeleteUserById(id);
        if (!deleteResponse) return BadRequest($"Could not delete user with ID: {id}");

        return Ok(deleteResponse);
    }
}