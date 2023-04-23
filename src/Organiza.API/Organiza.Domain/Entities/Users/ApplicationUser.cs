using Microsoft.AspNetCore.Identity;

namespace Organiza.Domain.Entities.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Cpf { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int? PasswordResetCode { get; set; }
        public DateTime? PasswordResetExpirationDate { get; set; }
        public string? AccessToken { get; set; }
        public string? Hash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; private set; }
        public bool Active { get; set; } = true;
        
        
        public void Delete()
        {
            this.Deleted = true;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}
