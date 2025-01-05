using UsersMicroservice.core.Application;
using UsersMicroservice.Core.Application;
using UsersMicroservice.Core.Common;
using UsersMicroservice.src.auth.application.commands.login.types;
using UsersMicroservice.src.auth.application.exceptions;
using UsersMicroservice.src.auth.application.repositories;
using UsersMicroservice.src.user.application.repositories;
using UsersMicroservice.src.user.application.repositories.exceptions;
using UsersMicroservice.src.user.domain.value_objects;

namespace UsersMicroservice.src.auth.application.commands.login
{
    public class LoginUserCommandHandler(ICryptoService cryptoService, ITokenAuthenticationService tokenAuthenticationService, ICredentialsRepository credentialsRepository, IUserRepository userRepository) : IApplicationService<LoginUserCommand, LoginUserResponse>
    {
        private readonly ICryptoService _cryptoService = cryptoService;
        private readonly ITokenAuthenticationService _tokenAuthenticationService = tokenAuthenticationService;
        private readonly ICredentialsRepository _credentialsRepository = credentialsRepository;
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Result<LoginUserResponse>> Execute(LoginUserCommand data)
        {
            var credentialsFind = await _credentialsRepository.GetCredentialsByEmail(data.Email);
            if (!credentialsFind.HasValue())
            {
                return Result<LoginUserResponse>.Failure(new CredentialsNotFoundByEmailException(data.Email));
            }
            var credentials = credentialsFind.Unwrap();
            if (!await _cryptoService.Compare(data.Password, credentials.Password))
            {
                return Result<LoginUserResponse>.Failure(new IncorrectPasswordException());
            }
            var userFind = await _userRepository.GetUserById(new UserId(credentials.UserId));
            if (!userFind.HasValue())
            {
                return Result<LoginUserResponse>.Failure(new UserNotFoundException());
            }
            var user = userFind.Unwrap();
            var token = _tokenAuthenticationService.Authenticate(user.GetName(), user.GetRole());
            return Result<LoginUserResponse>.Success(new LoginUserResponse(user, token.AccessToken, token.ExpiresIn));
        }
    }
}
