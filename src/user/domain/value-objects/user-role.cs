using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.user.domain.exceptions;
namespace UsersMicroservice.src.user.domain.value_objects
{
    public class UserRole : IValueObject<UserRole>
    {
        private readonly string _role;
        private readonly List<string> roles = ["admin", "operator", "provider", "conductor"];

        public UserRole(string role)
        {
            if (!roles.Contains(role.ToLower().Trim()))
            {
                throw new InvalidUserRoleException();
            }
            _role = role;
        }

        public string GetRole()
        {
            return _role;
        }

        public bool Equals(UserRole other)
        {
            return _role == other.GetRole();
        }
    }
}
