using UsersMicroservice.core.Common;
using UsersMicroservice.src.user.application.commands.update_user.types;
using UsersMicroservice.src.user.application.repositories.dto;
using UsersMicroservice.src.user.domain;
using UsersMicroservice.src.user.domain.value_objects;
namespace UsersMicroservice.src.user.application.repositories
{
    public interface IUserRepository
    {
        public Task<User> SaveUser(User user);
        public Task<_Optional<List<User>>> GetAllUsers(GetAllUsersDto data);
        public Task<_Optional<User>> GetUserById(UserId id);
        public Task<UserId> UpdateUserById(UpdateUserByIdCommand command);
        public Task<UserId> ToggleActivityUserById(UserId id);

    }
}
