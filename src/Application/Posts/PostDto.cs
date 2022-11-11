using Application.Profiles;

namespace Application.Posts;

public class PostDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime Date { get; set; }

    public Profile? Author { get; set; }
}
