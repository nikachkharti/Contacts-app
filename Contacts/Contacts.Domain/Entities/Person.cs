using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contacts.Domain.Entities
{
    public class Person
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
