using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.core.Application;
using UsersMicroservice.core.Infrastructure;
using UsersMicroservice.src.auth.application.commands.login;
using UsersMicroservice.src.auth.application.commands.login.types;
using UsersMicroservice.src.auth.application.commands.recover_password;
using UsersMicroservice.src.auth.application.commands.recover_password.types;
using UsersMicroservice.src.auth.application.commands.update_credentials;
using UsersMicroservice.src.auth.application.commands.update_credentials.types;
using UsersMicroservice.src.auth.application.repositories;
using UsersMicroservice.src.user.application.repositories;

namespace UsersMicroservice.src.auth.infrastructure
{
    [Route("auth")]
    [ApiController]
    public class AuthController(
        ICredentialsRepository credentialsRepository,
        IUserRepository userRepository,
        ICryptoService cryptoService,
        ITokenAuthenticationService tokenAuthenticationService,
        IConfiguration configuration) : Controller
    {
        private readonly ICredentialsRepository _credentialsRepository = credentialsRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICryptoService _cryptoService = cryptoService;
        private readonly ITokenAuthenticationService _tokenAuthenticationService = tokenAuthenticationService;
        private readonly IConfiguration _configuration = configuration;

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

        [HttpPatch("update-credentials")]
        [Authorize]
        public async Task<IActionResult> UpdateCredentials([FromBody] UpdateCredentialsCommand command)
        {
            var service = new UpdateCredentialsCommandHandler(_credentialsRepository, _cryptoService);
            var response = await service.Execute(command);
            if (response.IsFailure)
            {
                return BadRequest(new { errorMessage = response.ErrorMessage() });
            }
            return Ok( new {message = "Credentials has been updated successfully"});
        }

        [HttpPatch("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordCommand command)
        {
            var service = new RecoverPasswordCommandHandler(_credentialsRepository, _cryptoService);
            var response = await service.Execute(command);
            if (response.IsFailure)
            {
                return BadRequest(new { errorMessage = response.ErrorMessage() });
            }
            try
            {
                var emailSender = new EmailSenderService(_configuration);
                await emailSender.SendEmail(command.Email, new EmailContent("Recuperación de contraseña Gruas UCAB", 
                $@"
                Tu nueva contraseña de acceso a Gruas UCAB es: 
                {response.Unwrap().Password}
                Accede a Gruas UCAB y actualiza tu contraseña.

                Departamento de Tecnología. Gruas UCAB.
                "));
                return Ok(new { message = "The password has been recovered successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }
    }
}