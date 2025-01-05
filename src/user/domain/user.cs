using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.department.domain.value_objects;
using UsersMicroservice.src.user.domain.events;
using UsersMicroservice.src.user.domain.exceptions;
using UsersMicroservice.src.user.domain.value_objects;

namespace UsersMicroservice.src.user.domain
{
    public class User(UserId id) : AggregateRoot<UserId>(id)
    {
        private UserName _name;
        private UserPhone _phone;
        private UserRole _role;
        private DepartmentId _department;
        private bool _isActive = true;
        protected override void ValidateState()
        {
            if (_name == null || _phone == null || _role == null || _department == null)
            {
                throw new InvalidUserException();
            }
        }

        public string GetId()
        {
            return _id.GetId();
        }
        public string GetName()
        {
            return _name.GetName();
        }

        public string GetPhone()
        {
            return _phone.GetPhoneNumber();
        }

        public string GetRole()
        {
            return _role.GetRole();
        }
        public string GetDepartmentId()
        {
            return _department.GetId();
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public void ChangeStatus()
        {
            _isActive = !_isActive;
        }

        public static User Create(UserId id, UserName name, UserPhone phone, UserRole role, DepartmentId department)
        {
            User user = new(id);
            user.Apply(UserCreated.CreateEvent(id, name, phone, role, department));
            return user;
        }

        public void UpdateName(UserName name)
        {
            Apply(UserNameUpdated.CreateEvent(_id, name));
            Console.WriteLine("Ya aplico");
        }

        public void UpdatePhone(UserPhone phone)
        {
            Apply(UserPhoneUpdated.CreateEvent(_id, phone));
        }

        private void OnUserCreatedEvent(UserCreated Event)
        {
            _name = new UserName(Event.Name);
            _phone = new UserPhone(Event.Phone);
            _role = new UserRole(Event.Role);
            _department = new DepartmentId(Event.Department);
        }

        private void OnUserNameUpdatedEvent(UserNameUpdated Event)
        {
            Console.WriteLine("Ya reacciono");
            _name = new UserName(Event.Name);
        }

        private void OnUserPhoneUpdatedEvent(UserPhoneUpdated Event)
        {
            _phone = new UserPhone(Event.Phone);
        }
    }
}
