using MediatR;

namespace Organiza.Application.Features.Users.Users.Commands.InsertUsers
{
    public class InsertUserCommand : IRequest<Unit>
    {
        public InsertUserCommand(string name, string cpf, DateTime dateOfBirth, string password, string email, string phoneNumber)
        {
            Name = name;
            Cpf = cpf;
            DateOfBirth = dateOfBirth;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
