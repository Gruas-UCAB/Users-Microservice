using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.core.Application;
using UsersMicroservice.core.Infrastructure;
using UsersMicroservice.src.auth.application.repositories;
using UsersMicroservice.src.auth.infrastructure.repositories;
using UsersMicroservice.src.department.application.repositories;
using UsersMicroservice.src.department.infrastructure.repositories;
using UsersMicroservice.src.user.application.commands.create_user;
using UsersMicroservice.src.user.application.commands.create_user.types;
using UsersMicroservice.src.user.application.commands.update_user;
using UsersMicroservice.src.user.application.commands.update_user.types;
using UsersMicroservice.src.user.application.repositories;
using UsersMicroservice.src.user.application.repositories.dto;
using UsersMicroservice.src.user.application.repositories.exceptions;
using UsersMicroservice.src.user.infrastructure.dto;
using UsersMicroservice.src.user.infrastructure.repositories;
using UsersMicroservice.src.user.infrastructure.validators;

namespace UsersMicroservice.src.user.infrastructure
{
    [Route("user")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {   
        private readonly IUserRepository _userRepository = new MongoUserRepository();
        private readonly ICredentialsRepository _credentialsRepository = new MongoCredentialsRepository();
        private readonly IDepartmentRepository _departmentRepository = new MongoDepartmentRepository();
        private readonly IIdGenerator<string> _idGenerator = new UUIDGenerator();
        private readonly ICryptoService _cryptoservice = new BcryptService();

        [Authorize(Policy = "CreationalUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            var validator = new CreateUserCommandValidator();
            if (!validator.Validate(command).IsValid)
            {
                var errorMessages = validator.Validate(command).Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { errors = errorMessages });
            }
            var service = new CreateUserCommandHandler(_idGenerator, _cryptoservice, _userRepository, _departmentRepository, _credentialsRepository);
            var response = await service.Execute(command);
            if (response.IsFailure)
            {
                return BadRequest(new { errorMessage = response.ErrorMessage() });
            }
            var data = response.Unwrap().Id;
            return Created("Created", new { id = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersDto data)
        {
            var users = await _userRepository.GetAllUsers(data);
            if (!users.HasValue())
            {
                return NotFound(new { errorMessage = new NoUsersFoundException().Message });
            }
            var userList = users.Unwrap().Select(u => new
            {
                Id = u.GetId(),
                Name = u.GetName(),
                Phone = u.GetPhone(),
                Role = u.GetRole(),
                Department = u.GetDepartmentId(),
                IsActive = u.IsActive()
            }).ToList();
            return Ok(userList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetUserById(new domain.value_objects.UserId(id));
            if (!user.HasValue())
            {
                return NotFound(new { errorMessage = new UserNotFoundException().Message });
            }
            var userData = new
            {
                Id = user.Unwrap().GetId(),
                Name = user.Unwrap().GetName(),
                Phone = user.Unwrap().GetPhone(),
                Role = user.Unwrap().GetRole(),
                Department = user.Unwrap().GetDepartmentId(),
                IsActive = user.Unwrap().IsActive()
            };
            return Ok(userData);
        }

        [Authorize(Policy = "CreationalUser")]
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateUserById([FromBody] UpdateUserDto data, string id)
        {
            try
            {
                var validator = new UpdateUserByIdValidator();
                if (!validator.Validate(data).IsValid)
                {
                    var errorMessages = validator.Validate(data).Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new { errors = errorMessages });
                }
                if (data.Name == null && data.Phone == null)
                {
                    return BadRequest(new { errorMessage = "Name or phone is required" });
                }
                var service = new UpdateUserByIdCommandHandler(_userRepository);
                var command = new UpdateUserByIdCommand(id, data.Name, data.Phone);
                var response = await service.Execute(command);
                return Ok(new { message = "User has been updated"});
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }

        }

        //[Authorize(Policy = "CreationalUser")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> ToggleActivityUserById(string id)
        {
            try
            {
                var user = await GetUserById(id);
                await _userRepository.ToggleActivityUserById(new domain.value_objects.UserId(id));
                return Ok(new { message = "Activity status of user has been changed" });
            } catch( Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }

    }
}
