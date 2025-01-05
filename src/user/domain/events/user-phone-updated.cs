using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.user.domain.value_objects;

namespace UsersMicroservice.src.user.domain.events
{
    public class UserPhoneUpdatedEvent : DomainEvent<object>
    {
        public UserPhoneUpdatedEvent(string dispatcherId, string name, UserPhoneUpdated context) : base(dispatcherId, name, context) { }
    }
    public class UserPhoneUpdated(string phone)
    {
        public string Phone = phone;
        static public UserPhoneUpdatedEvent CreateEvent(UserId dispatcherId, UserPhone phone)
        {
            return new UserPhoneUpdatedEvent(
                dispatcherId.GetId(),
                typeof(UserPhoneUpdated).Name,
                new UserPhoneUpdated(
                    phone.GetPhoneNumber()
                )
            );
        }
    }
}
