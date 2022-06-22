using Domain;
using MediatR;
using Persistence;

namespace Application.Posts;

public class Create
{

    public class Command : IRequest
    {
        public Post Post { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;

        public Handler(DataContext _context)
        {
            this._context = _context;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            _context.Posts.Add(request.Post);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}