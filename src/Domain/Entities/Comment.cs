using System;

namespace Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }

    public string Body { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Author { get; set; }

    public Post Post { get; set; }
}
