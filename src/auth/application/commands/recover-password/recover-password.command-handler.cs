using System.Net;
using UsersMicroservice.core.Application;
using UsersMicroservice.Core.Application;
using UsersMicroservice.core.Common;
using UsersMicroservice.Core.Common;
using UsersMicroservice.src.auth.application.commands.recover_password.types;
using UsersMicroservice.src.auth.application.exceptions;
using UsersMicroservice.src.auth.application.repositories;

namespace UsersMicroservice.src.auth.application.commands.recover_password
{
    public class RecoverPasswordCommandHandler(ICredentialsRepository credentialsRepository, ICryptoService cryptoService) : IApplicationService<RecoverPasswordCommand, RecoverPasswordResponse>
    {
        private readonly ICredentialsRepository _credentialsRepository = credentialsRepository;
        private readonly ICryptoService _cryptoService = cryptoService;
        public async Task<Result<RecoverPasswordResponse>> Execute(RecoverPasswordCommand data)
        {
            if (!EmailValidator.IsValid(data.Email))
                return Result<RecoverPasswordResponse>.Failure(new InvalidEmailException());
            var credentialsFound = await _credentialsRepository.GetCredentialsByEmail(data.Email);
            if (!credentialsFound.HasValue())
                return Result<RecoverPasswordResponse>.Failure(new CredentialsNotFoundByEmailException(data.Email));
            var credentials = credentialsFound.Unwrap();
            var newPassword = RandomPasswordGenerator.GenerateRandomPassword();
            var hashedPassword = await _cryptoService.Hash(newPassword);
            await _credentialsRepository.UpdateCredentials(credentials.UserId, credentials.Email, hashedPassword);
            return Result<RecoverPasswordResponse>.Success(new RecoverPasswordResponse(newPassword));
        }
    }
}
