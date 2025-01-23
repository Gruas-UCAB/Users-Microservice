namespace UsersMicroservice.src.auth.application.exceptions
{
    public class CantUpdateCredentialsException : ApplicationException
    {
        public CantUpdateCredentialsException() : base("Can't update credentials, Invalid fields.")
        {
        }
    }
}
