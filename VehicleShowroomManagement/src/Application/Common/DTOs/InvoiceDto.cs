using System;
using System.Collections.Generic;

namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for Invoice entities
    /// </summary>
    public class InvoiceDto
    {
        public string Id { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceType { get; set; } = "Sales"; // Sales, Service
        public string? SalesOrderId { get; set; }
        public string? ServiceOrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Unpaid";
        public List<string> PaymentIds { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Related entity information
        public CustomerInfo? Customer { get; set; }
        public VehicleInfo? Vehicle { get; set; }
        public ServiceInfo? Service { get; set; }
        
        // Computed properties
        public bool IsServiceInvoice => InvoiceType == "Service";
        public bool IsSalesInvoice => InvoiceType == "Sales";
        public string OrderId => IsServiceInvoice ? ServiceOrderId ?? string.Empty : SalesOrderId ?? string.Empty;
    }

    /// <summary>
    /// Service information for invoices
    /// </summary>
    public class ServiceInfo
    {
        public string ServiceOrderId { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CompletedDate { get; set; }
        public string? TechnicianName { get; set; }
    }
}