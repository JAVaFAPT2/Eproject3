using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// VehicleSpec entity for vehicle specifications
    /// </summary>
    public class VehicleSpec
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("vehicleId")]
        [BsonRequired]
        public string VehicleId { get; private set; } = string.Empty;

        [BsonElement("specName")]
        [BsonRequired]
        public string SpecName { get; private set; } = string.Empty;

        [BsonElement("specValue")]
        [BsonRequired]
        public string SpecValue { get; private set; } = string.Empty;

        [BsonElement("displayOrder")]
        public int DisplayOrder { get; private set; }

        [BsonElement("groupName")]
        public string? GroupName { get; private set; }

        // Internal constructor for MongoDB
        internal VehicleSpec() { }

        [BsonConstructor]
        public VehicleSpec(string vehicleId, string specName, string specValue, 
            int displayOrder = 0, string? groupName = null)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

            if (string.IsNullOrWhiteSpace(specName))
                throw new ArgumentException("Spec name cannot be null or empty", nameof(specName));

            if (string.IsNullOrWhiteSpace(specValue))
                throw new ArgumentException("Spec value cannot be null or empty", nameof(specValue));

            VehicleId = vehicleId;
            SpecName = specName;
            SpecValue = specValue;
            DisplayOrder = displayOrder;
            GroupName = groupName;
        }

        // Domain methods
        public void UpdateValue(string specValue)
        {
            if (string.IsNullOrWhiteSpace(specValue))
                throw new ArgumentException("Spec value cannot be null or empty", nameof(specValue));

            SpecValue = specValue;
        }

        public void UpdateDisplayOrder(int displayOrder)
        {
            DisplayOrder = displayOrder;
        }

        public void UpdateGroupName(string? groupName)
        {
            GroupName = groupName;
        }
    }
}

