using UsersMicroservice.Core.Domain;
using UsersMicroservice.src.department.domain.exceptions;
namespace UsersMicroservice.src.department.domain.value_objects
{
    public class DepartmentName : IValueObject<DepartmentName>
    {
        private readonly string _name;

        public DepartmentName(string name)
        {
            if (name.Length < 2 || name.Length > 50)
            {
                throw new InvalidDepartmentNameException();
            }
            this._name = name;
        }
        public string GetName()
        {
            return this._name;
        }
        public bool Equals(DepartmentName other)
        {
            return _name == other.GetName();
        }
    }
}
