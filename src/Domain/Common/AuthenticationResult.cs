namespace Domain.Common;

public class AuthenticationResult
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
}