using API.Cache;
using API.Extensions;
using Application.Core.Queries;
using Application.Posts;
using AutoMapper;
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

        return HandlePagedResult<PostDto>(posts);
    }
    
    [HttpGet("{id:guid}")]
    [Cached(600)]
    public async Task<IActionResult> GetPost(Guid id)
    {
        var post = await Mediator.Send(new Details.Query { Id = id });

        return HandleResult(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDto post)
    {
        var postToCreate = Mapper.Map<Post>(post);
        postToCreate.UserId = HttpContext.GetUserId();

        return HandleResult(await Mediator.Send(new Create.Command { Post = postToCreate }));
    }

    [Authorize(Policy = "IsPostAuthor")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditPost(Guid id, [FromBody]PostDto post)
    {
        var postToEdit = Mapper.Map<Post>(post);
        postToEdit.UserId = HttpContext.GetUserId();
        postToEdit.Id = id;

        return HandleResult(await Mediator.Send(new Edit.Command { Post = postToEdit }));
    }
    
    [Authorize(Policy = "IsPostAuthor")]
    [HttpDelete]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }
}
