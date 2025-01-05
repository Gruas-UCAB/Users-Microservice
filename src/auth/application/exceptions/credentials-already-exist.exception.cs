using UsersMicroservice.Core.Domain;

namespace UsersMicroservice.src.auth.application.exceptions
{
    public class CredentialsAlreadyExistsException : DomainException
    {
        public CredentialsAlreadyExistsException() : base("Credentials already exists")
        {
        }
    }
}
