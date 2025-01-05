using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.user.domain.exceptions;
namespace UsersMicroservice.src.user.domain.value_objects
{
    public class UserName : IValueObject<UserName>
    {
        private readonly string _name;

        public UserName(string name)
        {
            if (name.Length < 2 || name.Length > 50)
            { 
            throw new InvalidUserNameException(); 
            }
            this._name = name;
        }
        public string GetName()
        {
            return this._name;
        }
        public bool Equals(UserName other)
        {
            return _name == other.GetName();
        }
    }
}
