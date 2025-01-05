using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.department.domain.events;
using UsersMicroservice.src.department.domain.exceptions;
using UsersMicroservice.src.department.domain.value_objects;

namespace UsersMicroservice.src.department.domain
{
    public class Department(DepartmentId id) : AggregateRoot<DepartmentId>(id)
    {
        private DepartmentId _id = id;
        private DepartmentName _name;
        public readonly bool IsActive = true;
        protected override void ValidateState()
        {
            if (_name == null )
            {
                throw new InvalidDepartmentException();
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

        public static Department Create(DepartmentId id, DepartmentName name)
        {
            Department department = new(id);
            department.Apply(DepartmentCreated.CreateEvent(id, name));
            return department;
        }

        private void OnDepartmentCreatedEvent(DepartmentCreated Event)
        {
            _name = new DepartmentName(Event.Name);
        }
    }
}
