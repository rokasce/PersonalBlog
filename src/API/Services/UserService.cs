using Domain;
using Domain.Common;
using Domain.Entities;
using Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services;

public class UserService: IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly DataContext _dataContext;
    private readonly ITokenService _tokenService;

    public UserService(UserManager<User> userManager, DataContext dataContext, ITokenService tokenService)
    {
        _userManager = userManager;
        _dataContext = dataContext;
        _tokenService = tokenService;
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return new AuthenticationResult
            {
                Errors = new []{ "User with this email already exists" }
            };
        }

        var newUser = new User
        {
            Email = email,
            UserName = email
        };

        var createdUser = await _userManager.CreateAsync(newUser, password);
        if (!createdUser.Succeeded)
        {
            return new AuthenticationResult
            {
                Errors = createdUser.Errors.Select(x => x.Description)
            };
        }

        return await GenerateAuthenticationResultAsync(newUser);
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new AuthenticationResult
            {
                Errors = new []{ "User does not exist" }
            };
        }

        var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!userHasValidPassword)
        {
            return new AuthenticationResult
            {
                Errors = new []{ "User password is incorrect" }
            };
        }

        return await GenerateAuthenticationResultAsync(user);
    }

    public async Task<AuthenticationResult> RefreshAsync(string refreshToken)
    {
/*        var validatedToken = _tokenService.GetPrincipalFromToken(token);
        if (validatedToken == null)
        {
            return new AuthenticationResult
            {
                Errors = new[] {"Invalid Token"}
            };
        }

        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);
        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            return new AuthenticationResult
            {
                Errors = new [] { "This token hasn't expired yet." }
            };
        }*/



        //var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var storedRefreshedToken = await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
        if (storedRefreshedToken == null)
        {
            return new AuthenticationResult { Errors = new []{ "This refresh token does not exit" }};
        }

        if (DateTime.UtcNow > storedRefreshedToken.ExpiryDate)
        {
            return new AuthenticationResult
            {
                Errors = new []{ "This refresh token has expired" }
            };
        }

        if (storedRefreshedToken.Invalidated)
        {
            return new AuthenticationResult { Errors = new []{ "This refresh token has been invalidated" }};
        }
        
        if (storedRefreshedToken.Used)
        {
            return new AuthenticationResult { Errors = new []{ "This refresh token has been used" }};
        }
        
/*        if (storedRefreshedToken.JwtId != jti)
        {
            return new AuthenticationResult { Errors = new []{ "This refresh token does not match this JWT" }};
        }*/

        storedRefreshedToken.Used = true;
        _dataContext.RefreshTokens.Update(storedRefreshedToken);
        await _dataContext.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(storedRefreshedToken.UserId);
        // var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
        return await GenerateAuthenticationResultAsync(user);
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = await _tokenService.GenerateTokenAsync(user);
        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        await _dataContext.RefreshTokens.AddAsync(refreshToken);
        await _dataContext.SaveChangesAsync();

        return new AuthenticationResult
        {
            Success = true,
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<bool> DeleteUserById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if(user == null) return false;

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded) return true;

        return false;
    }

    public async Task<List<User>> GetPagedUsersAsync() 
    {
        var users = await _userManager.Users.ToListAsync();

        return users;
    }
}