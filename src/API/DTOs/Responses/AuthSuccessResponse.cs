namespace API.DTOs.Responses;

public class AuthSuccessResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}