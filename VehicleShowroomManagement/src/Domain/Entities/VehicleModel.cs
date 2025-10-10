using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// VehicleModel entity representing vehicle models available for purchase
    /// ModelNumber is the primary key (_id in MongoDB)
    /// </summary>
    public class VehicleModel
    {
        [BsonId]
        public string ModelNumber { get; private set; } = string.Empty;

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; private set; } = string.Empty;

        [BsonElement("brand")]
        [BsonRequired]
        public string Brand { get; private set; } = string.Empty;

        [BsonElement("price")]
        [BsonRequired]
        public decimal Price { get; private set; }

        // Parameterless constructor for MongoDB deserialization
        public VehicleModel() { }

        public VehicleModel(string modelNumber, string name, string brand, decimal price)
        {
            if (string.IsNullOrWhiteSpace(modelNumber))
                throw new ArgumentException("Model number cannot be null or empty", nameof(modelNumber));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be null or empty", nameof(brand));

            if (price < 0)
                throw new ArgumentException("Price cannot be negative", nameof(price));

            ModelNumber = modelNumber;
            Name = name;
            Brand = brand;
            Price = price;
        }

        // Domain methods
        public void UpdateModel(string name, string brand, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be null or empty", nameof(brand));

            if (price < 0)
                throw new ArgumentException("Price cannot be negative", nameof(price));

            Name = name;
            Brand = brand;
            Price = price;
        }
    }
}
