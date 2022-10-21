using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Posts;

public class List
{
    public class Query : IRequest<IEnumerable<Post>> { }

    public class Handler : IRequestHandler<Query,IEnumerable<Post>>
    {
        private readonly DataContext _context;
        
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Posts.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}