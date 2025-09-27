using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// DealerAvailability entity for managing dealer time slots
    /// </summary>
    public class DealerAvailability
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("dealerId")]
        [BsonRequired]
        public string DealerId { get; private set; } = string.Empty;

        [BsonElement("date")]
        [BsonRequired]
        public DateTime Date { get; private set; }

        [BsonElement("startTime")]
        [BsonRequired]
        public TimeSpan StartTime { get; private set; }

        [BsonElement("endTime")]
        [BsonRequired]
        public TimeSpan EndTime { get; private set; }

        [BsonElement("isAvailable")]
        public bool IsAvailable { get; private set; } = true;

        [BsonElement("maxAppointments")]
        public int MaxAppointments { get; private set; } = 8;

        [BsonElement("currentAppointments")]
        public int CurrentAppointments { get; private set; } = 0;

        [BsonElement("breakTime")]
        public TimeSpan? BreakTime { get; private set; }

        [BsonElement("breakDuration")]
        public int BreakDurationMinutes { get; private set; } = 60;

        [BsonElement("notes")]
        public string? Notes { get; private set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private DealerAvailability() { }

        public DealerAvailability(
            string dealerId,
            DateTime date,
            TimeSpan startTime,
            TimeSpan endTime,
            int maxAppointments = 8,
            TimeSpan? breakTime = null,
            int breakDurationMinutes = 60)
        {
            if (string.IsNullOrWhiteSpace(dealerId))
                throw new ArgumentException("Dealer ID cannot be null or empty", nameof(dealerId));

            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time", nameof(startTime));

            if (maxAppointments <= 0)
                throw new ArgumentException("Max appointments must be greater than zero", nameof(maxAppointments));

            DealerId = dealerId;
            Date = date.Date; // Ensure only date part
            StartTime = startTime;
            EndTime = endTime;
            MaxAppointments = maxAppointments;
            BreakTime = breakTime;
            BreakDurationMinutes = breakDurationMinutes;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void BookSlot()
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Dealer is not available");

            if (CurrentAppointments >= MaxAppointments)
                throw new InvalidOperationException("Maximum appointments reached for this time slot");

            CurrentAppointments++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelSlot()
        {
            if (CurrentAppointments > 0)
            {
                CurrentAppointments--;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void SetUnavailable(string? reason = null)
        {
            IsAvailable = false;
            Notes = reason;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAvailable()
        {
            IsAvailable = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateMaxAppointments(int maxAppointments)
        {
            if (maxAppointments <= 0)
                throw new ArgumentException("Max appointments must be greater than zero", nameof(maxAppointments));

            MaxAppointments = maxAppointments;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetBreakTime(TimeSpan breakTime, int durationMinutes)
        {
            BreakTime = breakTime;
            BreakDurationMinutes = durationMinutes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        // Helper methods
        public bool HasAvailableSlots => IsAvailable && CurrentAppointments < MaxAppointments;
        public int AvailableSlots => IsAvailable ? Math.Max(0, MaxAppointments - CurrentAppointments) : 0;
        public bool IsWorkingHours(TimeSpan time) => time >= StartTime && time <= EndTime;
        public bool IsBreakTime(TimeSpan time) => BreakTime.HasValue && 
            time >= BreakTime.Value && 
            time <= BreakTime.Value.Add(TimeSpan.FromMinutes(BreakDurationMinutes));
    }
}