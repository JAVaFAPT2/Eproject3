using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Domain.Entities
{
    /// <summary>
    /// Appointment aggregate root for customer appointment booking
    /// </summary>
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("appointmentNumber")]
        [BsonRequired]
        public string AppointmentNumber { get; private set; } = string.Empty;

        [BsonElement("customerId")]
        [BsonRequired]
        public string CustomerId { get; private set; } = string.Empty;

        [BsonElement("dealerId")]
        [BsonRequired]
        public string DealerId { get; private set; } = string.Empty;

        [BsonElement("appointmentDate")]
        [BsonRequired]
        public DateTime AppointmentDate { get; private set; }

        [BsonElement("duration")]
        public int DurationMinutes { get; private set; } = 60;

        [BsonElement("appointmentType")]
        public AppointmentType Type { get; private set; } = AppointmentType.Consultation;

        [BsonElement("status")]
        public AppointmentStatus Status { get; private set; } = AppointmentStatus.Scheduled;

        [BsonElement("customerRequirements")]
        public string? CustomerRequirements { get; private set; }

        [BsonElement("vehicleInterest")]
        public string? VehicleInterest { get; private set; }

        [BsonElement("budgetRange")]
        public string? BudgetRange { get; private set; }

        [BsonElement("notes")]
        public string? Notes { get; private set; }

        [BsonElement("dealerNotes")]
        public string? DealerNotes { get; private set; }

        [BsonElement("followUpRequired")]
        public bool FollowUpRequired { get; private set; } = false;

        [BsonElement("followUpDate")]
        public DateTime? FollowUpDate { get; private set; }

        [BsonElement("reminderSent")]
        public bool ReminderSent { get; private set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        // Private constructor for MongoDB
        private Appointment() { }

        public Appointment(
            string appointmentNumber,
            string customerId,
            string dealerId,
            DateTime appointmentDate,
            AppointmentType type,
            int durationMinutes = 60,
            string? customerRequirements = null,
            string? vehicleInterest = null,
            string? budgetRange = null)
        {
            if (string.IsNullOrWhiteSpace(appointmentNumber))
                throw new ArgumentException("Appointment number cannot be null or empty", nameof(appointmentNumber));

            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));

            if (string.IsNullOrWhiteSpace(dealerId))
                throw new ArgumentException("Dealer ID cannot be null or empty", nameof(dealerId));

            if (appointmentDate <= DateTime.UtcNow)
                throw new ArgumentException("Appointment date must be in the future", nameof(appointmentDate));

            AppointmentNumber = appointmentNumber;
            CustomerId = customerId;
            DealerId = dealerId;
            AppointmentDate = appointmentDate;
            Type = type;
            DurationMinutes = durationMinutes;
            CustomerRequirements = customerRequirements;
            VehicleInterest = vehicleInterest;
            BudgetRange = budgetRange;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void Confirm(string? dealerNotes = null)
        {
            if (Status != AppointmentStatus.Scheduled)
                throw new InvalidOperationException("Only scheduled appointments can be confirmed");

            Status = AppointmentStatus.Confirmed;
            DealerNotes = dealerNotes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel(string? reason = null)
        {
            if (Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Completed appointments cannot be cancelled");

            Status = AppointmentStatus.Cancelled;
            Notes = reason;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reschedule(DateTime newDate)
        {
            if (newDate <= DateTime.UtcNow)
                throw new ArgumentException("New appointment date must be in the future", nameof(newDate));

            if (Status != AppointmentStatus.Scheduled && Status != AppointmentStatus.Confirmed)
                throw new InvalidOperationException("Only scheduled or confirmed appointments can be rescheduled");

            AppointmentDate = newDate;
            Status = AppointmentStatus.Scheduled; // Reset to scheduled
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete(string? dealerNotes = null)
        {
            if (Status != AppointmentStatus.Confirmed && Status != AppointmentStatus.InProgress)
                throw new InvalidOperationException("Only confirmed or in-progress appointments can be completed");

            Status = AppointmentStatus.Completed;
            DealerNotes = dealerNotes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Start()
        {
            if (Status != AppointmentStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed appointments can be started");

            Status = AppointmentStatus.InProgress;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetFollowUp(DateTime followUpDate, bool required = true)
        {
            FollowUpRequired = required;
            FollowUpDate = followUpDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkReminderSent()
        {
            ReminderSent = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateCustomerRequirements(string requirements, string? vehicleInterest = null, string? budgetRange = null)
        {
            CustomerRequirements = requirements;
            if (!string.IsNullOrEmpty(vehicleInterest))
                VehicleInterest = vehicleInterest;
            if (!string.IsNullOrEmpty(budgetRange))
                BudgetRange = budgetRange;
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

        // Computed properties
        public bool IsUpcoming => AppointmentDate > DateTime.UtcNow && Status == AppointmentStatus.Scheduled;
        public bool IsToday => AppointmentDate.Date == DateTime.UtcNow.Date;
        public bool IsOverdue => AppointmentDate < DateTime.UtcNow && Status != AppointmentStatus.Completed && Status != AppointmentStatus.Cancelled;
        public bool CanBeModified => Status == AppointmentStatus.Scheduled || Status == AppointmentStatus.Confirmed;
    }
}