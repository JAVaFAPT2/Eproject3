namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for Excel generation service
    /// </summary>
    public interface IExcelService
    {
        /// <summary>
        /// Generates Excel file from data
        /// </summary>
        Task<byte[]> GenerateExcelAsync<T>(List<T> data, string worksheetName, string fileName);

        /// <summary>
        /// Generates Excel file with multiple worksheets
        /// </summary>
        Task<byte[]> GenerateExcelWithMultipleSheetsAsync(Dictionary<string, object> worksheets, string fileName);
    }
}