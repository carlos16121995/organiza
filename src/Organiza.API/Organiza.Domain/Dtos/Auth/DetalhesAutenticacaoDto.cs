using System.Security.Principal;

namespace Organiza.Domain.Dtos.Auth
{
    public class AuthenticationDetailDto : IIdentity
    {
        public string? AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? Name { get; set; }
    }
}
