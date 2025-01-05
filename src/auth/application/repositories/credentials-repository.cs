using UsersMicroservice.core.Common;
using UsersMicroservice.src.auth.application.models;
namespace UsersMicroservice.src.auth.application.repositories
{
    public interface ICredentialsRepository
    {
        Task<_Optional<Credentials>> GetCredentialsByEmail(string email);
        Task<_Optional<Credentials>> GetCredentialsByUserId(string userId);
        Task<string> AddCredentials(Credentials credentials);
    }
}