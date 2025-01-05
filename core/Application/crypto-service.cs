namespace UsersMicroservice.core.Application
{
    public interface ICryptoService
    {
        Task<string> Hash(string password);
        Task<bool> Compare(string text, string hashedText);
    }
}
