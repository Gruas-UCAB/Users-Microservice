using UsersMicroservice.core.Application;
using UsersMicroservice.Core.Application;
using UsersMicroservice.Core.Common;
using UsersMicroservice.src.auth.application.repositories;
using UsersMicroservice.src.department.application.repositories;
using UsersMicroservice.src.department.application.repositories.exceptions;
using UsersMicroservice.src.department.domain.value_objects;
using UsersMicroservice.src.user.application.commands.create_user.types;
using UsersMicroservice.src.user.application.repositories;
using UsersMicroservice.src.user.domain;
using UsersMicroservice.src.user.domain.value_objects;
using UsersMicroservice.src.auth.application.models;
using UsersMicroservice.src.auth.application.exceptions;

namespace UsersMicroservice.src.user.application.commands.create_user
{
    public class CreateUserCommandHandler(
        IIdGenerator<string> idGenerator,
        ICryptoService cryptoService,
        IUserRepository userRepository, 
        IDepartmentRepository departmentRepository, 
        ICredentialsRepository credentialsRepository) : IApplicationService<CreateUserCommand, CreateUserResponse>
    {
        private readonly IIdGenerator<string> _idGenerator = idGenerator;
        private readonly ICryptoService _cryptoService = cryptoService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly ICredentialsRepository _credentialsRepository = credentialsRepository;

        public async Task<Result<CreateUserResponse>> Execute(CreateUserCommand data)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentById(new DepartmentId(data.Department));
                if (!department.HasValue())
                {
                    return Result<CreateUserResponse>.Failure(new DepartmentNotFoundException());
                }
                var credentialsExists = await _credentialsRepository.GetCredentialsByEmail(data.Email);
                if (credentialsExists.HasValue())
                {
                    return Result<CreateUserResponse>.Failure(new CredentialsAlreadyExistsException());
                }
                var user = User.Create(
                    new UserId(_idGenerator.GenerateId()),
                    new UserName(data.Name),
                    new UserPhone(data.Phone),
                    new UserRole(data.Role),
                    new DepartmentId(data.Department)
                    );
                var password = await _cryptoService.Hash(data.Password);
                await _credentialsRepository.AddCredentials(new Credentials(_idGenerator.GenerateId(), user.GetId(), data.Email.ToLower(), password));
                await _userRepository.SaveUser(user);
                return Result<CreateUserResponse>.Success(new CreateUserResponse(user.GetId()));
            } catch (Exception e)
            {
                return Result<CreateUserResponse>.Failure(e);
            }
        }
    }
}
