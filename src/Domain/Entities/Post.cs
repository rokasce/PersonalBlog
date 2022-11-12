using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain;

public class Post
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime Date { get; set; } 
    public string UserId { get; set; }
    public User User { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}