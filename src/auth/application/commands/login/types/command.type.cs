namespace UsersMicroservice.src.auth.application.commands.login.types
{
    public record LoginUserCommand
    (
        string Email,
        string Password
    );
}
