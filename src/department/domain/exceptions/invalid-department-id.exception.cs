using UsersMicroservice.Core.Domain;
namespace UsersMicroservice.src.department.domain.exceptions
{
    public class InvalidDepartmentIdException : DomainException
{
    public InvalidDepartmentIdException() : base("Invalid department ID")
    {
    }
}
}
