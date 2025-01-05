using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.user.domain.value_objects;

namespace UsersMicroservice.src.user.domain.events
{
    public class UserNameUpdatedEvent : DomainEvent<object>
    {
        public UserNameUpdatedEvent(string dispatcherId, string name, UserNameUpdated context) : base(dispatcherId, name, context) { }
    }

    public class UserNameUpdated(string name)
    {
        public string Name = name;
        static public UserNameUpdatedEvent CreateEvent(UserId dispatcherId, UserName name)
        {
            Console.WriteLine("No aplico");
            return new UserNameUpdatedEvent(
                dispatcherId.GetId(),
                typeof(UserNameUpdated).Name,
                new UserNameUpdated(
                    name.GetName()
                )
            );
        }
    }
}
