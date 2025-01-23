namespace UsersMicroservice.src.auth.application.exceptions
{
    public class CredentialsNotFoundException : ApplicationException
    {
        public CredentialsNotFoundException(): base ("Credentials not found") { }
    }
}
