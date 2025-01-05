using UsersMicroservice.Core.Domain;
namespace UsersMicroservice.src.department.domain.exceptions
{
    public class InvalidDepartmentException : DomainException
    {
        public InvalidDepartmentException() : base("Invalid department")
        {
        }
    }
}
