namespace UsersMicroservice.src.auth.application.commands.update_credentials.types
{
    public record UpdateCredentialsCommand
    (
        string UserId,
        string? Email,
        string? Password
    );
}
