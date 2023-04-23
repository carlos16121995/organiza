using Microsoft.EntityFrameworkCore;
using Organiza.Domain.Entities.Users;

namespace Organiza.Infrastructure.Persistence
{
    public partial class Context
    {
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    }
}
