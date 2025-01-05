namespace UsersMicroservice.src.user.application.repositories.dto
{
    public record GetAllUsersDto
    (
        int limit = 10,
        int offset = 1,
        bool active = true
    );
}
