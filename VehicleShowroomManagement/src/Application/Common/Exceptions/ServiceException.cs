namespace VehicleShowroomManagement.Application.Common.Exceptions
{
    /// <summary>
    /// Base exception for service operations
    /// </summary>
    public abstract class ServiceException : Exception
    {
        protected ServiceException(string message) : base(message) { }
        protected ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception for Cloudinary service operations
    /// </summary>
    public class CloudinaryException : ServiceException
    {
        public CloudinaryException(string message) : base(message) { }
        public CloudinaryException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception for Email service operations
    /// </summary>
    public class EmailException : ServiceException
    {
        public EmailException(string message) : base(message) { }
        public EmailException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception for PDF generation operations
    /// </summary>
    public class PdfGenerationException : ServiceException
    {
        public PdfGenerationException(string message) : base(message) { }
        public PdfGenerationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception for Excel generation operations
    /// </summary>
    public class ExcelGenerationException : ServiceException
    {
        public ExcelGenerationException(string message) : base(message) { }
        public ExcelGenerationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
