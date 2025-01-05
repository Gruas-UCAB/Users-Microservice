using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsersMicroservice.src.auth.infrastructure.models
{
    public class MongoCredentials
    {
        [BsonId]
        [BsonElement("id"), BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("userId"), BsonRepresentation(BsonType.String)]
        public required string UserId { get; set; }

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public required string Email { get; set; }

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public required string Password { get; set; }

        [BsonElement("createdAt"), BsonRepresentation(BsonType.String)]
        public required DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt"), BsonRepresentation(BsonType.String)]
        public required DateTime UpdatedAt { get; set; }
    }
}
