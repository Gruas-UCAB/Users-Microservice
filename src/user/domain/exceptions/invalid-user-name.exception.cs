using UsersMicroservice.Core.Domain;
namespace UsersMicroservice.src.user.domain.exceptions
{
    public class InvalidUserNameException : DomainException
    {
        public InvalidUserNameException() : base("Invalid user name")
        {
        }
    }
}
