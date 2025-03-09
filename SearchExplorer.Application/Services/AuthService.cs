using SearchExplorer.Core.Interfaces;

namespace SearchExplorer.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(JwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            // Here you should validate the user credentials from a database
            if (username == "testuser" && password == "password123") // Replace with DB check
            {
                return await Task.FromResult(_jwtTokenGenerator.GenerateToken(username));
            }

            throw new UnauthorizedAccessException("Invalid username or password.");
        }
    }
}
