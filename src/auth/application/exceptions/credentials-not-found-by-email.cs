namespace UsersMicroservice.src.auth.application.exceptions
{
    public class CredentialsNotFoundByEmailException : ApplicationException 
    {
        public CredentialsNotFoundByEmailException(string email) : base($"Credentials not found by email: {email}") { }
    }
}
