namespace UsersMicroservice.src.auth.application.exceptions
{
    public class IncorrectPasswordException : ApplicationException
    {
        public IncorrectPasswordException() : base("Incorrect password")
        {
        }
    }
}
