using UsersMicroservice.Core.Domain;

using UsersMicroservice.src.department.domain.exceptions;

namespace UsersMicroservice.src.department.domain.value_objects;
public class DepartmentId : IValueObject<DepartmentId>
{
    private readonly string _id;
    public DepartmentId(string id)
    {
        if (UUIDValidator.IsValid(id))
        {
            _id = id;
        }
        else
        {
            throw new InvalidDepartmentIdException();
        }
    }

    public string GetId()
    {
        return _id;
    }

    public bool Equals(DepartmentId other)
    {
        return _id == other.GetId();
    }
}
