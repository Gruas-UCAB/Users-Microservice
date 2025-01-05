namespace UsersMicroservice.src.department.application.repositories.exceptions
{
    public class DepartmentNotFoundException : Exception
    {
        public DepartmentNotFoundException() : base("Department not found") { }
    }
}
