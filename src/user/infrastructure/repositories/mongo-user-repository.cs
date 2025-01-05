using MongoDB.Bson;
using MongoDB.Driver;
using UsersMicroservice.core.Common;
using UsersMicroservice.core.Infrastructure;
using UsersMicroservice.src.department.domain.value_objects;
using UsersMicroservice.src.user.application.commands.update_user.types;
using UsersMicroservice.src.user.application.repositories;
using UsersMicroservice.src.user.application.repositories.dto;
using UsersMicroservice.src.user.application.repositories.exceptions;
using UsersMicroservice.src.user.domain;
using UsersMicroservice.src.user.domain.value_objects;
using UsersMicroservice.src.user.infrastructure.models;

namespace UsersMicroservice.src.user.infrastructure.repositories
{
    public class MongoUserRepository : IUserRepository
    {
        internal MongoDBConfig _config = new();
        private readonly IMongoCollection<BsonDocument> userCollection;

        public MongoUserRepository()
        {
            userCollection = _config.db.GetCollection<BsonDocument>("users");
        }

        public async Task<User> SaveUser(User user)
        {
            var mongoUser = new MongoUser
            {
                Id = user.GetId(),
                Name = user.GetName(),
                Phone = user.GetPhone(),
                Role = user.GetRole(),
                Department = user.GetDepartmentId(),
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            var bsonDocument = new BsonDocument
            {
                {"_id", mongoUser.Id},
                {"name", mongoUser.Name},
                {"phone", mongoUser.Phone },
                {"role", mongoUser.Role},
                {"department", mongoUser.Department},
                {"isActive", mongoUser.IsActive},
                {"createdAt", mongoUser.CreatedAt },
                {"updatedAt", mongoUser.UpdatedAt }
            };

            await userCollection.InsertOneAsync(bsonDocument);

            var savedUser = User.Create(
                new UserId(mongoUser.Id), 
                new UserName(mongoUser.Name), 
                new UserPhone(mongoUser.Phone), 
                new UserRole(mongoUser.Role), 
                new DepartmentId(mongoUser.Department));

            return savedUser;
        }
        public async Task<_Optional<List<User>>> GetAllUsers(GetAllUsersDto data)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("isActive", data.active);
            var users = await userCollection.Find(filter).Limit(data.limit).Skip(data.limit * (data.offset - 1)).ToListAsync();

            var userList = new List<User>();

            foreach (var bsonUser in users)
            {
                var user = User.Create(
                new UserId(bsonUser["_id"].AsString),
                new UserName(bsonUser["name"].AsString),
                new UserPhone(bsonUser["phone"].AsString),
                new UserRole(bsonUser["role"].AsString),
                new DepartmentId(bsonUser["department"].AsString)
                );

                if (!bsonUser["isActive"].AsBoolean)
                {
                    user.ChangeStatus();
                }
                userList.Add(user);
                
            }

            if (userList.Count == 0)
            {
                return _Optional<List<User>>.Empty();
            }
            return _Optional<List<User>>.Of(userList);
        }

        public async Task<_Optional<User>> GetUserById(UserId id)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id.GetId());
                var bsonUser = await userCollection.Find(filter).FirstOrDefaultAsync();
        
                if (bsonUser == null)
                {
                    return _Optional<User>.Empty();
                }

                var user = User.Create(
                    new UserId(bsonUser["_id"].AsString),
                    new UserName(bsonUser["name"].AsString),
                    new UserPhone(bsonUser["phone"].AsString),
                    new UserRole(bsonUser["role"].AsString),
                    new DepartmentId(bsonUser["department"].AsString)
                );

                if (!bsonUser["isActive"].AsBoolean)
                {
                    user.ChangeStatus();
                }

                return _Optional<User>.Of(user);
            }
            catch (Exception ex)
            {
                return _Optional<User>.Empty();
            }
        }

        public async Task<UserId> UpdateUserById(UpdateUserByIdCommand command)
        {
            var user = await GetUserById(new UserId(command.Id));
            if (!user.HasValue())
            {
                throw new UserNotFoundException();
            }
            
            var filter = Builders<BsonDocument>.Filter.Eq("_id", command.Id);

            var update = Builders<BsonDocument>.Update
                .Set("updatedAt", DateTime.Now);

            if (command.Name != null)
            {
                update = update.Set("name", command.Name);
            }
            if (command.Phone != null)
            {
                update = update.Set("phone", command.Phone);
            }

            await userCollection.UpdateOneAsync(filter, update);

            return new UserId(command.Id);
        }

        public async Task<UserId> ToggleActivityUserById(UserId id)
        {
            var user = await GetUserById(id);
            if (!user.HasValue())
            {
                throw new UserNotFoundException();
            }

            var userToUpdate = user.Unwrap();
            userToUpdate.ChangeStatus();

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id.GetId());
            var update = Builders<BsonDocument>.Update.
                Set("isActive", userToUpdate.IsActive())
                .Set("updatedAt", DateTime.Now);

            await userCollection.UpdateOneAsync(filter, update);

            return id;
        }

    }
}
