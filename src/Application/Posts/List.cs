using Application.Core;
using Application.Core.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Posts;

public class List
{
    public class Query : IRequest<Result<PagedList<PostDto>>> 
    {
        public PaginationQuery? PaginationQuery { get; init; }

        public Query(PaginationQuery? paginationQuery)
        {
            PaginationQuery = paginationQuery;
        }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<PostDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<PostDto>>> Handle(Query request, CancellationToken cancellationToken)
        {

            var query = _context.Posts
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.Date)
                .AsQueryable();

            var posts = await PagedList<PostDto>.CreateAsync(query,
                request.PaginationQuery.PageNumber, request.PaginationQuery.PageSize);

            return Result<PagedList<PostDto>>.Success(posts);
        }
    }
}