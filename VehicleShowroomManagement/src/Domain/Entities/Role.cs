using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Role entity for role-based access control
    /// </summary>
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; private set; } = string.Empty;

        // Internal constructor for MongoDB
        internal Role() { }

        [BsonConstructor]
        public Role(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name cannot be null or empty", nameof(name));

            Name = name;
        }

        // Domain methods
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name cannot be null or empty", nameof(name));

            Name = name;
        }
    }
}
