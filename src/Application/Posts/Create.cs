using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Posts;

public class Create
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

        public Handler(DataContext _context)
        {
            this._context = _context;
        }
        
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            _context.Posts.Add(request.Post);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return result 
                ? Result<Unit>.Success(Unit.Value) 
                : Result<Unit>.Failure("Failed to create post");
        }
    }
}