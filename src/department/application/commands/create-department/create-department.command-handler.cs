using UsersMicroservice.core.Application;
using UsersMicroservice.Core.Application;
using UsersMicroservice.Core.Common;
using UsersMicroservice.src.department.application.commands.create_department.types;
using UsersMicroservice.src.department.application.repositories;
using UsersMicroservice.src.department.domain;
using UsersMicroservice.src.department.domain.value_objects;

namespace UsersMicroservice.src.department.application.commands.create_user
{
    public class CreateDepartmentCommandHandler(IIdGenerator<string> idGenerator, IDepartmentRepository departmentRepository) : IApplicationService<CreateDepartmentCommand, CreateDepartmentResponse>
    {
        private readonly IIdGenerator<string> _idGenerator = idGenerator;
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        public async Task<Result<CreateDepartmentResponse>> Execute(CreateDepartmentCommand data)
        {
            try
            {
                var id = _idGenerator.GenerateId();
                var name = data.name;

                var department = Department.Create(
                    new DepartmentId(id),
                    new DepartmentName(name)
                    );
                var events = department.PullEvents();
                await _departmentRepository.SaveDepartment(department);
                return Result<CreateDepartmentResponse>.Success(new CreateDepartmentResponse(id));
            }
            catch (Exception e)
            {
                return Result<CreateDepartmentResponse>.Failure(e);
            }
        }
    }
}
