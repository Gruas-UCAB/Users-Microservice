using UsersMicroservice.Core.Common;
using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.user.domain.exceptions;
namespace UsersMicroservice.src.user.domain.value_objects
{
    public class UserPhone : IValueObject<UserPhone>
    {
        private readonly string _phoneNumber;

        public UserPhone(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || !PhoneNumberValidator.IsValid(phoneNumber))
            {
                throw new InvalidUserPhoneException();
            }
            this._phoneNumber = phoneNumber;
        }

        public string GetPhoneNumber()
        {
            return this._phoneNumber;
        }

        public bool Equals(UserPhone other)
        {
            return this._phoneNumber == other._phoneNumber;
        }
    }
}
