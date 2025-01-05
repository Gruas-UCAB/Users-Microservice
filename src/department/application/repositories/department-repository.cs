using UsersMicroservice.core.Common;
using UsersMicroservice.src.department.domain;
using UsersMicroservice.src.department.domain.value_objects;

namespace UsersMicroservice.src.department.application.repositories
{
    public interface IDepartmentRepository
    {
        public Task<Department> SaveDepartment(Department department);
        public Task<_Optional<List<Department>>> GetAllDepartments();
        public Task<_Optional<Department>> GetDepartmentById(DepartmentId id);

    }
}
