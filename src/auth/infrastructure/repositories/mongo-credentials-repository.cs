using MongoDB.Bson;
using MongoDB.Driver;
using UsersMicroservice.core.Common;
using UsersMicroservice.core.Infrastructure;
using UsersMicroservice.src.auth.application.models;
using UsersMicroservice.src.auth.application.repositories;
using UsersMicroservice.src.auth.infrastructure.models;
using UsersMicroservice.src.user.application.repositories.exceptions;
using UsersMicroservice.src.user.domain;
using UsersMicroservice.src.user.domain.value_objects;

namespace UsersMicroservice.src.auth.infrastructure.repositories
{
    public class MongoCredentialsRepository : ICredentialsRepository
    {
        internal MongoDBConfig _config = new();
        private readonly IMongoCollection<BsonDocument> credentialsCollection;
        public MongoCredentialsRepository()
        {
            credentialsCollection = _config.db.GetCollection<BsonDocument>("credentials");
        }
        public async Task<string> AddCredentials(Credentials credentials)
        {
            var mongoCredentials = new MongoCredentials
            {
                Id = credentials.Id,
                UserId = credentials.UserId,
                Email = credentials.Email,
                Password = credentials.Password,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            var bsonDocument = new BsonDocument
            {
                {"_id", mongoCredentials.Id},
                {"userId", mongoCredentials.UserId},
                {"email", mongoCredentials.Email },
                {"password", mongoCredentials.Password},
                {"createdAt", mongoCredentials.CreatedAt },
                {"updatedAt", mongoCredentials.UpdatedAt }
            };

            await credentialsCollection.InsertOneAsync(bsonDocument);

            return credentials.Id;
        }

        public async Task UpdateCredentials(string userId, string? email, string? password)
        {
            var credentialsFind = await GetCredentialsByUserId(userId);
            if (!credentialsFind.HasValue())
                throw new UserNotFoundException();
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var update = Builders<BsonDocument>.Update.Set("updatedAt", DateTime.Now);
            if (email != null)
            {
                update = update.Set("email", email);
            }
            if (password != null)
            {
                update = update.Set("password", password);
            }
            await credentialsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<_Optional<Credentials>> GetCredentialsByEmail(string email)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("email", email);
                var bsonCredentials = await credentialsCollection.Find(filter).FirstOrDefaultAsync();
                if (bsonCredentials == null)
                {
                    return _Optional<Credentials>.Empty();
                }
                var credentials = new Credentials(
                    bsonCredentials["_id"].AsString,
                    bsonCredentials["userId"].AsString,
                    bsonCredentials["email"].AsString,
                    bsonCredentials["password"].AsString
                    );
                return _Optional<Credentials>.Of(credentials);
            }
            catch (Exception e)
            {
                return _Optional<Credentials>.Empty();
            }
        }

        public async Task<_Optional<Credentials>> GetCredentialsByUserId(string userId)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
                var bsonCredentials = await credentialsCollection.Find(filter).FirstOrDefaultAsync();
                if (bsonCredentials == null)
                {
                    return _Optional<Credentials>.Empty();
                }
                var credentials = new Credentials(
                    bsonCredentials["_id"].AsString, 
                    bsonCredentials["userId"].AsString, 
                    bsonCredentials["email"].AsString, 
                    bsonCredentials["password"].AsString
                    );
                return _Optional<Credentials>.Of(credentials);
            }
            catch (Exception e)
            {
                return _Optional<Credentials>.Empty();
            }
        }
    }
}
