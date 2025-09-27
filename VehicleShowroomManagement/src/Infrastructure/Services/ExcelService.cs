using OfficeOpenXml;
using System.Reflection;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Implementation of Excel generation service using EPPlus
    /// </summary>
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> GenerateExcelAsync<T>(List<T> data, string worksheetName, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(worksheetName);

            if (data.Any())
            {
                // Get properties of the type
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Add headers
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                // Add data
                for (int row = 0; row < data.Count; row++)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(data[row]);
                        worksheet.Cells[row + 2, col + 1].Value = value;
                    }
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();
            }

            return await Task.FromResult(package.GetAsByteArray());
        }

        public async Task<byte[]> GenerateExcelWithMultipleSheetsAsync(Dictionary<string, object> worksheets, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();

            foreach (var worksheet in worksheets)
            {
                var ws = package.Workbook.Worksheets.Add(worksheet.Key);
                
                // This would need to be implemented based on the specific data types
                // For now, just adding the worksheet
            }

            return await Task.FromResult(package.GetAsByteArray());
        }
    }
}