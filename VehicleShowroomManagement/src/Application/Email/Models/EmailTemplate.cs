namespace VehicleShowroomManagement.Application.Email.Models
{
    /// <summary>
    /// Email template model for different business processes
    /// </summary>
    public class EmailTemplate
    {
        public string TemplateName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public Dictionary<string, object> Variables { get; set; } = new();
    }

    /// <summary>
    /// Email template types for different business processes
    /// </summary>
    public static class EmailTemplateTypes
    {
        // Authentication Templates
        public const string PasswordReset = "PasswordReset";
        public const string WelcomeEmail = "WelcomeEmail";
        public const string AccountCreated = "AccountCreated";

        // Order Templates
        public const string OrderConfirmation = "OrderConfirmation";
        public const string OrderStatusUpdate = "OrderStatusUpdate";
        public const string OrderCancellation = "OrderCancellation";

        // Purchase Order Templates
        public const string PurchaseOrderCreated = "PurchaseOrderCreated";
        public const string PurchaseOrderApproved = "PurchaseOrderApproved";
        public const string PurchaseOrderRejected = "PurchaseOrderRejected";

        // Vehicle Templates
        public const string VehicleReady = "VehicleReady";
        public const string VehicleRegistration = "VehicleRegistration";
        public const string ServiceCompleted = "ServiceCompleted";

        // Customer Templates
        public const string CustomerWelcome = "CustomerWelcome";
        public const string AllotmentConfirmation = "AllotmentConfirmation";
        public const string WaitingListNotification = "WaitingListNotification";

        // System Templates
        public const string SystemNotification = "SystemNotification";
        public const string ReportGenerated = "ReportGenerated";
        public const string BackupCompleted = "BackupCompleted";
    }
}
