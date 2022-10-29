using API.Cache;
using API.Extensions;
using Application.Core;
using Application.Core.Queries;
using Application.Posts;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PostsController: BaseApiController
{
    [HttpGet]
    [Cached(600)]
    public async Task<IActionResult> GetPosts([FromQuery]PaginationQuery paginationQuery)
    {
        var posts = await Mediator.Send(new List.Query(paginationQuery));

        return HandlePagedResult<Post>(posts);
    }
    
    [HttpGet("{id:guid}")]
    [Cached(600)]
    public async Task<IActionResult> GetPost(Guid id)
    {
        var post = await Mediator.Send(new Details.Query { Id = id });

        return HandleResult(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody]Post post)
    {
        post.UserId = HttpContext.GetUserId();
        return Ok(await Mediator.Send(new Create.Command { Post = post }));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditPost(Guid id, [FromBody]Post post)
    {
        post.Id = id;
        return Ok(await Mediator.Send(new Edit.Command { Post = post }));
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        return Ok(await Mediator.Send(new Delete.Command { Id = id }));
    }
}