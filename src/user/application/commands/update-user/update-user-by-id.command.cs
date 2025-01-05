using MongoDB.Bson.Serialization;
using UsersMicroservice.core.Application;
using UsersMicroservice.Core.Application;
using UsersMicroservice.Core.Common;
using UsersMicroservice.src.user.application.commands.update_user.types;
using UsersMicroservice.src.user.application.repositories;
using UsersMicroservice.src.user.application.repositories.exceptions;

namespace UsersMicroservice.src.user.application.commands.update_user
{
    public class UpdateUserByIdCommandHandler(IUserRepository userRepository) : IApplicationService<UpdateUserByIdCommand, UpdateUserByIdResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Result<UpdateUserByIdResponse>> Execute(UpdateUserByIdCommand data)
        {
            try
            {
                var user = await _userRepository.GetUserById(new domain.value_objects.UserId(data.Id));
                if (!user.HasValue())
                {
                    return Result<UpdateUserByIdResponse>.Failure(new UserNotFoundException());
                }

                var userToUpdate = user.Unwrap();
                if (data.Name != null)
                {
                    userToUpdate.UpdateName(new domain.value_objects.UserName(data.Name));
                    Console.WriteLine("Cambio nombre");
                }
                if (data.Phone != null)
                {
                    Console.WriteLine("Cambio telefono");
                    userToUpdate.UpdatePhone(new domain.value_objects.UserPhone(data.Phone));
                }

                await _userRepository.UpdateUserById(data);

                return Result<UpdateUserByIdResponse>.Success(new UpdateUserByIdResponse(userToUpdate.GetId()));
            }
            catch (Exception e) 
            {
                return Result<UpdateUserByIdResponse>.Failure(e);
            }
        }
    }
}
