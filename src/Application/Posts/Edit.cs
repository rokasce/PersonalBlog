using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Posts;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Post Post { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Post).SetValidator(new PostValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(request.Post.Id);

            if (post == null) return null;

            _mapper.Map(request.Post, post);

            var result = await _context.SaveChangesAsync() > 0;
         
            return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Could not edit the post");
        }
    } 
}