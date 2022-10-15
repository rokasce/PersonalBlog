using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext: IdentityDbContext<User>
{
    public DataContext(DbContextOptions options): base(options)
    {
        
    }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}