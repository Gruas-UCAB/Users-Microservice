using UsersMicroservice.core.Application;
using UsersMicroservice.Core.Application;
using UsersMicroservice.core.Common;
using UsersMicroservice.Core.Common;
using UsersMicroservice.src.auth.application.commands.update_credentials.types;
using UsersMicroservice.src.auth.application.exceptions;
using UsersMicroservice.src.auth.application.repositories;

namespace UsersMicroservice.src.auth.application.commands.update_credentials
{
    public class UpdateCredentialsCommandHandler(ICredentialsRepository credentialsRepository, ICryptoService cryptoService) : IApplicationService<UpdateCredentialsCommand, UpdateCredentialsResponse>
    {
        private readonly ICredentialsRepository _credentialsRepository = credentialsRepository;
        private readonly ICryptoService _cryptoService = cryptoService;
        public async Task<Result<UpdateCredentialsResponse>> Execute(UpdateCredentialsCommand data)
        {
            if (data.Email == null && data.Password == null)
            {
                return Result<UpdateCredentialsResponse>.Failure(new CantUpdateCredentialsException());
            }
            var credentialsFound = await _credentialsRepository.GetCredentialsByUserId(data.UserId);
            if (!credentialsFound.HasValue())
            {
                return Result<UpdateCredentialsResponse>.Failure(new CredentialsNotFoundException());
            }
            var credentials = credentialsFound.Unwrap();
            if (data.Email != null && !EmailValidator.IsValid(data.Email))
                return Result<UpdateCredentialsResponse>.Failure(new InvalidEmailException());
            if (data.Password != null)
            {
                var hashedPassword = await _cryptoService.Hash(data.Password);
                await _credentialsRepository.UpdateCredentials(data.UserId, data.Email, hashedPassword);
            }
            else
            {
                await _credentialsRepository.UpdateCredentials(data.UserId, data.Email, data.Password);
            }
            return Result<UpdateCredentialsResponse>.Success(new UpdateCredentialsResponse(credentials.UserId));
        }
    }
}
