using MediatR;

namespace SearchExplorer.Application.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public string Username { get; }
        public string Password { get; }

        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
