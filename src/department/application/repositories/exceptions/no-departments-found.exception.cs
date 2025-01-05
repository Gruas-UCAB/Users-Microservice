namespace UsersMicroservice.src.department.application.repositories.exceptions
{
    public class NoDepartmentsFoundException : Exception
    {
        public NoDepartmentsFoundException() : base("No departments found")
        {
        }
    }
}
