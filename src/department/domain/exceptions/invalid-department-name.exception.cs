using UsersMicroservice.Core.Domain;
namespace UsersMicroservice.src.department.domain.exceptions
{
    public class InvalidDepartmentNameException : DomainException
    {
        public InvalidDepartmentNameException() : base("Invalid department name")
        {
        }
    }
}
