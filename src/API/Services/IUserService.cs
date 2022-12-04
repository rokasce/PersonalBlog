using Domain.Common;
using Domain.Entities;

namespace API.Services;

public interface IUserService
{
    Task<AuthenticationResult> RegisterAsync(string email, string password);
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> RefreshAsync (string requestRefreshToken);
    Task<bool> DeleteUserById(Guid id);
    Task<List<User>> GetPagedUsersAsync();
}