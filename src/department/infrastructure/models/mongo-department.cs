using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace UsersMicroservice.src.department.infrastructure.models
{
    public class MongoDepartment
    {
        [BsonId]
        [BsonElement("id"), BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public required string Name { get; set; }

        [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
        public required DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; }
    }
}
