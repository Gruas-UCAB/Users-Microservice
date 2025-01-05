namespace UsersMicroservice.src.user.application.commands.create_user.types
{
    public record CreateUserCommand(
        string Name,
        string Phone,
        string Role,
        string Department,
        string Email,
        string Password
     );
}
