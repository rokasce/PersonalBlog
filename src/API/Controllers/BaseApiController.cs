using API.Extensions;
using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator 
        ??= HttpContext.RequestServices.GetService<IMediator>()!;

    private IMapper _mapper;
    protected IMapper Mapper => _mapper 
        ??= HttpContext.RequestServices.GetService<IMapper>()!;


    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value != null) return Ok(result.Value);
        if (result.IsSuccess && result.Value == null) return NotFound();

        return BadRequest(result.Error);
    }

    protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value != null)
        {
            Response.AddPaginationHeaders(result.Value.CurrentPage,
                result.Value.PageSize, result.Value.TotalCount, result.Value.TotalPages);
            return Ok(result.Value);
        }

        if (result.IsSuccess && result.Value == null) return NotFound();

        return BadRequest(result.Error);
    }
}
