using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.core.Application;
using UsersMicroservice.core.Infrastructure;
using UsersMicroservice.src.department.application.commands.create_department.types;
using UsersMicroservice.src.department.application.commands.create_user;
using UsersMicroservice.src.department.application.repositories;
using UsersMicroservice.src.department.application.repositories.exceptions;
using UsersMicroservice.src.department.infrastructure.repositories;
using UsersMicroservice.src.department.infrastructure.validators;

namespace UsersMicroservice.src.department.infrastructure
{
    [Route("department")]
    [ApiController]
    [Authorize]
    public class DepartmentController(IDepartmentRepository departmentRepository, IIdGenerator<string> idGenerator) : Controller
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly IIdGenerator<string> _idGenerator = idGenerator;

        [Authorize(Policy = "CreationalUser")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentCommand command)
        {
            var validator = new CreateDepartmentCommandValidator();
            if(!validator.Validate(command).IsValid)
            {
                var errorMessages = validator.Validate(command).Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { errors = errorMessages });
            }
            var service = new CreateDepartmentCommandHandler(_idGenerator, _departmentRepository);
            var response = await service.Execute(command);
            if (response.IsFailure)
            {
                return BadRequest(new { errorMessage = response.ErrorMessage() });
            }
            var data = response.Unwrap().Id;
            return Created("Created", new {id = data} );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentRepository.GetAllDepartments();

            if (!departments.HasValue())
            {
                return NotFound(
                    new { errorMessage = new DepartmentNotFoundException().Message }
                    );
            }

            var departmentList = departments.Unwrap().Select(d => new
            {
                Id = d.GetId(),
                Name = d.GetName()
            }).ToList();

            return Ok(departmentList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(string id)
        {
            var department = await _departmentRepository.GetDepartmentById(new domain.value_objects.DepartmentId(id));
            if (!department.HasValue())
            {
                return NotFound(
                    new {errorMessage = new DepartmentNotFoundException().Message }
                    );
            }

            var departmentData = new
            {
                Id = department.Unwrap().GetId(),
                Name = department.Unwrap().GetName()
            };

            return Ok(departmentData);
        }
    }
}
