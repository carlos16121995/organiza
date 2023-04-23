using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organiza.Domain.Entities.Users;

namespace Organiza.Infrastructure.Persistence.Configurations.Users
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .ToTable(nameof(ApplicationUser), "Users")
                .HasKey(t => t.Id);

            builder
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(e => e.AccessToken)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder
                .HasIndex(e => e.NormalizedEmail, "EmailIndex");

            builder
                .HasIndex(e => e.Email, "IX_ApplicationUser_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            builder
                .HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique();

            builder
                .Property(e => e.Cpf)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder
                .Property((e) => e.UpdatedAt)
                .HasDefaultValueSql("GetUtcDate()")
                .ValueGeneratedOnAdd();

            builder
                .Property(e => e.PasswordResetExpirationDate)
                .HasColumnType("datetime");

            builder
                .Property(e => e.DateOfBirth)
                .HasColumnType("date");

            builder
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);

            builder
                .Property(e => e.NormalizedUserName)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);

            builder
                .Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false);

            builder
                .Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(16)
                .IsUnicode(false);

            builder
                .Property(e => e.SecurityStamp)
                .IsRequired()
                .HasMaxLength(64)
                .IsUnicode(false);
        }
    }
}
