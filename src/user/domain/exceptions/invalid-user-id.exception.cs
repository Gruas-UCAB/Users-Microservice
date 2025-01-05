using UsersMicroservice.Core.Domain;
namespace UsersMicroservice.src.user.domain.exceptions
{
    public class InvalidUserIdException : DomainException
{
    public InvalidUserIdException() : base("Invalid user ID")
    {
    }
}
}
