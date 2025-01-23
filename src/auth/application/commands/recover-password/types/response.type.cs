namespace UsersMicroservice.src.auth.application.commands.recover_password.types
{
    public class RecoverPasswordResponse(string password)
    {
        public readonly string Password = password;
    }
}
