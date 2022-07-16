namespace API.DTOs.Responses;

public class AuthFailedResponse
{
    public IEnumerable<string> Errors { get; set; }
}