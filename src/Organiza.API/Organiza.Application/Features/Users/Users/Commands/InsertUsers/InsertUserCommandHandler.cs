using MediatR;
using Organiza.Infrastructure.Persistence;

namespace Organiza.Application.Features.Users.Users.Commands.InsertUsers
{
    public class InsertUserCommandHandler : IRequestHandler<InsertUserCommand, Unit>
    {
        private readonly Context _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public InsertUserCommandHandler(Context context) => _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Unit> Handle(InsertUserCommand request, CancellationToken cancellationToken)
        {
            _context.ApplicationUsers.Add(new()
            {
            });
            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
