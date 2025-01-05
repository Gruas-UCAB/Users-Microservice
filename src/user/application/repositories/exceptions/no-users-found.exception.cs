namespace UsersMicroservice.src.user.application.repositories.exceptions
{
    public class NoUsersFoundException : Exception
    {
        public NoUsersFoundException() : base("No users found") { }
    }
}
