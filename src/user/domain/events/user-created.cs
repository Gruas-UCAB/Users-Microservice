using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.department.domain.value_objects;
using UsersMicroservice.src.user.domain.value_objects;

namespace UsersMicroservice.src.user.domain.events
{
    public class UserCreatedEvent : DomainEvent<object>
    {   
        public UserCreatedEvent(string dispatcherId, string name, UserCreated context) : base(dispatcherId, name, context){ }
    }
    public class UserCreated(string name, string phone, string role, string department)
    {
        public string Name = name;
        public string Phone = phone;
        public string Role = role;
        public string Department = department;
        static public UserCreatedEvent CreateEvent(UserId dispatcherId, UserName name, UserPhone phone, UserRole role, DepartmentId department)
        {
            return new UserCreatedEvent(
                dispatcherId.GetId(),
                typeof (UserCreated).Name,
                new UserCreated(
                    name.GetName(),
                    phone.GetPhoneNumber(),
                    role.GetRole(),
                    department.GetId()
                )
            );
        }      

    }
}



