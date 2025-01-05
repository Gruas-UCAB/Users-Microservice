using UsersMicroservice.core.Application;

namespace UsersMicroservice.core.Infrastructure
{
    public class BcryptService : ICryptoService
    {
        public Task<bool> Compare(string text, string hashedText)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.EnhancedVerify(text, hashedText));
        }

        public Task<string> Hash(string password)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.EnhancedHashPassword(password, 11));
        }
    }
}
