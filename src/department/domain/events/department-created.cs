using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.department.domain.value_objects;
namespace UsersMicroservice.src.department.domain.events
{
    public class DepartmentCreatedEvent : DomainEvent<object>
        {   
        public DepartmentCreatedEvent(string dispatcherId, string name, DepartmentCreated context) : base(dispatcherId, name, context){ }
    }

    public class DepartmentCreated(string name)
    {
        public string Name = name;
        static public DepartmentCreatedEvent CreateEvent(DepartmentId dispatcherId, DepartmentName name)
        {
            return new DepartmentCreatedEvent(
                dispatcherId.GetId(),
                typeof(DepartmentCreated).Name,
                new DepartmentCreated(
                    name.GetName()
                )
            );
        }
    }
}



