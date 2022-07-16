using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Options;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Services;

public class UserService: IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly DataContext _dataContext;

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, DataContext dataContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
        _dataContext = dataContext;
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
        // TODO: Make messages so it wont give info what wrong happened
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

    public async Task<AuthenticationResult> RefreshAsync(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token);

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
        }

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
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
        
        if (storedRefreshedToken.JwtId != jti)
        {
            return new AuthenticationResult { Errors = new []{ "This refresh token does not match this JWT" }};
        }

        storedRefreshedToken.Used = true;
        _dataContext.RefreshTokens.Update(storedRefreshedToken);
        await _dataContext.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
        return await GenerateAuthenticationResultAsync(user);
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                return null;
            }

            return principal;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture);
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id),
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6)
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
}