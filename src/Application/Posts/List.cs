using Application.Core;
using Application.Core.Queries;
using Domain;
using MediatR;
using Persistence;

namespace Application.Posts;

public class List
{
    public class Query : IRequest<Result<PagedList<Post>>> 
    {
        public PaginationQuery? PaginationQuery { get; init; }

        public Query(PaginationQuery? paginationQuery)
        {
            PaginationQuery = paginationQuery;
        }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<Post>>>
    {
        private readonly DataContext _context;
        
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<PagedList<Post>>> Handle(Query request, CancellationToken cancellationToken)
        {

            var query = _context.Posts.OrderBy(x => x.Date).AsQueryable();

            return Result<PagedList<Post>>.Success(await PagedList<Post>.CreateAsync(query,
                request.PaginationQuery.PageNumber, request.PaginationQuery.PageSize));
        }
    }
}