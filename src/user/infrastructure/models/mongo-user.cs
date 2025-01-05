using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsersMicroservice.src.user.infrastructure.models
{
    public class MongoUser
    {
        [BsonId]
        [BsonElement("id"), BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public required string Name { get; set; }

        [BsonElement("phone"), BsonRepresentation(BsonType.String)]
        public required string Phone { get; set; }

        [BsonElement("role"), BsonRepresentation(BsonType.String)]
        public required string Role { get; set; }

        [BsonElement("department"), BsonRepresentation(BsonType.String)]
        public required string Department { get; set; }

        [BsonElement("isActive"), BsonRepresentation(BsonType.Boolean)]
        public required bool IsActive { get; set; }

        [BsonElement("createdAt"), BsonRepresentation(BsonType.String)]
        public required DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt"), BsonRepresentation(BsonType.String)]
        public required DateTime UpdatedAt { get; set; }
    }
}
