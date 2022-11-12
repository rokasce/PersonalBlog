using Application.Core;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace Application.Comments;

public class Create
{
    public class Command : IRequest<Result<CommentDto>> 
    {
        public string Body { get; set; }    
        public Guid PostId { get; set; }    
    }

    public class CommandValidator : AbstractValidator<Command> 
    {
        public CommandValidator()
        {
            RuleFor(c => c.Body).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<CommentDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(DataContext dataContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = await _dataContext.Posts.FindAsync(request.PostId);
            if (post == null) return null;

            var user = await _dataContext.Users
                .SingleOrDefaultAsync(x => x.UserName == _httpContextAccessor.HttpContext.User.FindFirstValue("id"));

            if (user == null) return null;

            var comment = new Comment 
            {
                Author = user,
                Post = post,
                Body = request.Body
            };

            post.Comments.Add(comment);

            var success = await _dataContext.SaveChangesAsync() > 0;
            if (success) 
            {
                return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
            }

            return Result<CommentDto>.Failure("Failed to add comment");
        }
    }
}
