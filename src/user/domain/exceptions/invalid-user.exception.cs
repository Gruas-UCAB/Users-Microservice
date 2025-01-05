using UsersMicroservice.Core.Domain;
namespace UsersMicroservice.src.user.domain.exceptions
{
    public class InvalidUserException : DomainException
    {
        public InvalidUserException() : base("Invalid user")
        {
        }
    }
}
