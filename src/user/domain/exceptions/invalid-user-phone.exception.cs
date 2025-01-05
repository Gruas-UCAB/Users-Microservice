using UsersMicroservice.Core.Domain;
namespace UsersMicroservice.src.user.domain.exceptions
{
    public class InvalidUserPhoneException : DomainException
    {
        public InvalidUserPhoneException() : base("Invalid user phone")
        {
        }
    }
}
