using MongoDB.Bson;
using MongoDB.Driver;
using UsersMicroservice.core.Common;
using UsersMicroservice.core.Infrastructure;
using UsersMicroservice.Core.Common;
using UsersMicroservice.src.department.application.repositories;
using UsersMicroservice.src.department.domain;
using UsersMicroservice.src.department.domain.value_objects;
using UsersMicroservice.src.department.infrastructure.models;

namespace UsersMicroservice.src.department.infrastructure.repositories
{
    public class MongoDepartmentRepository : IDepartmentRepository
    {
        internal MongoDBConfig _config = new();
        private readonly IMongoCollection<BsonDocument> departmentCollection;

        public MongoDepartmentRepository()
        {
            departmentCollection = _config.db.GetCollection<BsonDocument>("departments");
        }

        public async Task<_Optional<List<Department>>> GetAllDepartments()
        {

            var departments = await departmentCollection.Find(new BsonDocument()).ToListAsync();
            var departmentList = departments.Select(department => Department.Create(
                new DepartmentId(department["_id"].AsString),
                new DepartmentName(department["name"].AsString)
            )).ToList();

            if (departmentList.Count == 0)
            {
                return _Optional<List<Department>>.Empty();
            }

            return _Optional<List<Department>>.Of(departmentList);

        }

        public async Task<_Optional<Department>> GetDepartmentById(DepartmentId id)
        {
            var bsonDepartment = await departmentCollection.FindAsync(new BsonDocument { { "_id", id.GetId() } }).Result.FirstOrDefaultAsync();
            if (bsonDepartment == null)
            {
                return _Optional<Department>.Empty();
            }
            var department = Department.Create(
                    new DepartmentId(bsonDepartment["_id"].AsString),
                    new DepartmentName(bsonDepartment["name"].AsString)
                    );
            return _Optional<Department>.Of(department);
        }

        public async Task<Department> SaveDepartment(Department department)
        {

            var mongoDepartment = new MongoDepartment
            {
                Id = department.GetId(),
                Name = department.GetName(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var bsonDocument = new BsonDocument
            {
                {"_id", mongoDepartment.Id},
                {"name", mongoDepartment.Name},
                {"createdAt", mongoDepartment.CreatedAt },
                {"updatedAt", mongoDepartment.UpdatedAt }
            };

            await departmentCollection.InsertOneAsync(bsonDocument);

            var savedDepartment = Department.Create(new DepartmentId(mongoDepartment.Id), new DepartmentName(mongoDepartment.Name));
            return savedDepartment; 
        }
    }
}
