using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Posts;

public class Details
{
    public class Query : IRequest<Result<PostDto>>
    {
        public Guid Id { get; init; }
    }

    public class Handler : IRequestHandler<Query, Result<PostDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return Result<PostDto>.Success(post);
        }
    }
}