﻿using API.Cache;
using API.Extensions;
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
    public async Task<ActionResult<List<Post>>> GetPosts()
    {
        return await Mediator.Send(new List.Query());
    }
    
    [HttpGet("{id:guid}")]
    [Cached(600)]
    public async Task<ActionResult<Post>> GetPost(Guid id)
    {
        return await Mediator.Send(new Details.Query { Id = id });
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