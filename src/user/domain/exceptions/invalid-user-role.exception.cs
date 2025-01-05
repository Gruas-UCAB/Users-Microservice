using UsersMicroservice.Core.Domain;

namespace UsersMicroservice.src.user.domain.exceptions
{
    public class InvalidUserRoleException : DomainException
    {
        public InvalidUserRoleException() : base("Invalid user role")
        {
        }
    }
}
