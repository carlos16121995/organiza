using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Organiza.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organiza.Infrastructure.Persistence.Configurations.Users;
using Organiza.Domain.Config;

namespace Organiza.Infrastructure.Persistence
{
    public partial class Context : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public Context(DbContextOptions<Context> opcoes) : base(opcoes) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Settings.Database.ConnectinString);
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserConfiguration).Assembly,
                    (type) => (type.Namespace ?? "").Contains("Configurations"));
        }
    }
}
