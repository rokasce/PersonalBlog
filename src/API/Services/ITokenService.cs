using Domain.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public interface ITokenService
{
    ClaimsPrincipal? GetPrincipalFromToken(string token);
    Task<SecurityToken> GenerateTokenAsync(User user);
}
