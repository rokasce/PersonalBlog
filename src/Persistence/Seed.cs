using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence;

public class Seed
{
   public static async Task SeedData(DataContext context, UserManager<User> userManager)
   {
      if (!userManager.Users.Any())
      {
         var users = new List<User>
         {
            new User
            {
               UserName= "bob@test.com",
               Email = "bob@test.com",
            },
            new User
            {
               UserName= "tom@test.com",
               Email = "tom@test.com",
            },
            new User
            {
               UserName= "jane@test.com",
               Email = "jane@test.com",
            }
         };
         
         foreach (var user in users)
         {
            await userManager.CreateAsync(user, "Test1234!");
         }
      }

      if (context.Posts.Any()) return;

      var posts = new List<Post>
      {
         new Post
         {
            Date = DateTime.Now.AddMonths(-2),
            Content = "Post #1 Content",
            Title = "Post #1 Title",
            UserId = context.Users.First().Id
         },
         new Post
         {
            Date = DateTime.Now.AddMonths(-1),
            Content = "Post #2 Content",
            Title = "Post #2 Title",
            UserId = context.Users.First().Id
         },
         new Post
         {
            Date = DateTime.Now,
            Content = "Post #3 Content",
            Title = "Post #3 Title",
            UserId = context.Users.First().Id
         },
         new Post
         {
            Date = DateTime.Now.AddMonths(1),
            Content = "Post #4 Content",
            Title = "Post #4 Title",
            UserId = context.Users.First().Id
         },
         new Post
         {
            Date = DateTime.Now.AddMonths(2),
            Content = "Post #5 Content",
            Title = "Post #5 Title",
            UserId = context.Users.First().Id
         },
      };

      await context.Posts.AddRangeAsync(posts);
      await context.SaveChangesAsync();
   }
}