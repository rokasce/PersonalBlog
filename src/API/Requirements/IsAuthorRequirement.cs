using Microsoft.AspNetCore.Authorization;
using Persistence;
using System.Security.Claims;

namespace API.Requirements;

public class IsAuthorRequirement : IAuthorizationRequirement
{

}

public class IsAuthorRequirementHandler : AuthorizationHandler<IsAuthorRequirement>
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsAuthorRequirementHandler(DataContext dataContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _dataContext = dataContext;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthorRequirement requirement)
    {
        var userId = context.User.FindFirstValue("id");
        if (userId == null) return Task.CompletedTask;

        if (_httpContextAccessor.HttpContext == null) return Task.CompletedTask;

        var postId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues
            .SingleOrDefault(x => x.Key == "id").Value?.ToString()!);

        var post = _dataContext.Posts.FindAsync(postId).Result;

        if (post == null) return Task.CompletedTask;

        if (post.UserId == userId) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
