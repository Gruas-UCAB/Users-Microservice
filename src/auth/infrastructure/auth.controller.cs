using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.core.Application;
using UsersMicroservice.core.Infrastructure;
using UsersMicroservice.src.auth.application.commands.login;
using UsersMicroservice.src.auth.application.commands.login.types;
using UsersMicroservice.src.auth.application.repositories;
using UsersMicroservice.src.auth.infrastructure.repositories;
using UsersMicroservice.src.user.application.repositories;
using UsersMicroservice.src.user.infrastructure.repositories;

namespace UsersMicroservice.src.auth.infrastructure
{
    [Route("auth")]
    [ApiController]
  
    public class AuthController : Controller
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly ITokenAuthenticationService _tokenAuthenticationService;

        public AuthController(
            ICredentialsRepository credentialsRepository,
            IUserRepository userRepository,
            ICryptoService cryptoService,
            ITokenAuthenticationService tokenAuthenticationService)
        {
            _credentialsRepository = credentialsRepository;
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _tokenAuthenticationService = tokenAuthenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var service = new LoginUserCommandHandler(_cryptoService, _tokenAuthenticationService, _credentialsRepository, _userRepository);
            var response = await service.Execute(command);
            if (response.IsFailure)
            {
                return Unauthorized(new { errorMessage = response.ErrorMessage() });
            }
            var data = response.Unwrap();
            return Ok(new
            {
                user = new
                {
                    userId = data.User.GetId(),
                    Name = data.User.GetName(),
                    Phone = data.User.GetPhone(),
                    Role = data.User.GetRole(),
                    Department = data.User.GetDepartmentId(),
                    IsActive = data.User.IsActive()
                },
                token = data.AccessToken,
                expiresIn = data.ExpiresIn
            });
        }
    }
}