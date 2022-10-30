using Application.Core;
using MediatR;
using Persistence;

namespace Application.Posts;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(request.Id);

            if (post == null) return null;

            _context.Posts.Remove(post);
            var result = await _context.SaveChangesAsync() > 0;

            return result 
                ? Result<Unit>.Success(Unit.Value) 
                : Result<Unit>.Failure("Could not delete the post");
        }
    }
}