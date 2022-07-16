using MediatR;
using Persistence;

namespace Application.Posts;

public class Delete
{
   public class Command : IRequest
   {
      public Guid Id { get; set; }
   }

   public class Handler : IRequestHandler<Command>
   {
      private readonly DataContext _context;

      public Handler(DataContext context)
      {
         _context = context;
      }


      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
         var post = await _context.Posts.FindAsync(request.Id);

         if (post is not null)
         {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
         }
         
         return Unit.Value;
      }
   }
}