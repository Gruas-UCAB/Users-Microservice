namespace UsersMicroservice.src.user.application.repositories.exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found") { }
    }
}
