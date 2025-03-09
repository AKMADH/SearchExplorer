using MediatR;
using SearchExplorer.Application.Commands;
using SearchExplorer.Core.Interfaces;

namespace SearchExplorer.Application.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IAuthService _authService;

        public LoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.AuthenticateAsync(request.Username, request.Password);
        }
    }
}
