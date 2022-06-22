using Domain;

namespace API.Services;

public interface IUserService
{
    Task<AuthenticationResult> RegisterAsync(string email, string password);
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> RefreshAsync(string requestToken, string requestRefreshToken);
}