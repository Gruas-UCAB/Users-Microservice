namespace UsersMicroservice.src.auth.application.exceptions
{
    public class InvalidEmailException : ApplicationException
    {
        public InvalidEmailException() : base("Invalid email"){}
    }
}
