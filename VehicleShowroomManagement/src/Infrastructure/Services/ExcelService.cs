using OfficeOpenXml;
using System.Reflection;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Implementation of Excel generation service using EPPlus
    /// </summary>
    public class ExcelService : BaseService, IExcelService
    {
        public ExcelService(ILogger<ExcelService> logger) : base(logger)
        {
        }
        public async Task<byte[]> GenerateExcelAsync<T>(List<T> data, string worksheetName, string fileName)
        {
            LogOperationStart(nameof(GenerateExcelAsync), new { worksheetName, fileName, dataCount = data.Count });

            try
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

                var result = await Task.FromResult(package.GetAsByteArray());
                LogOperationComplete(nameof(GenerateExcelAsync), new { worksheetName, fileName, dataCount = data.Count, fileSize = result.Length });
                return result;
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(GenerateExcelAsync), ex, new { worksheetName, fileName, dataCount = data.Count });
                throw new ExcelGenerationException($"Failed to generate Excel file: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> GenerateExcelWithMultipleSheetsAsync(Dictionary<string, object> worksheets, string fileName)
        {
            LogOperationStart(nameof(GenerateExcelWithMultipleSheetsAsync), new { fileName, sheetCount = worksheets.Count });

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using var package = new ExcelPackage();

                foreach (var worksheet in worksheets)
                {
                    var ws = package.Workbook.Worksheets.Add(worksheet.Key);
                    
                    // Handle different data types using reflection
                    var data = worksheet.Value;
                    if (data is IEnumerable<object> enumerableData)
                    {
                        var enumerableList = enumerableData.ToList();
                        if (enumerableList.Any())
                        {
                            var firstItem = enumerableList.First();
                            var properties = firstItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                            // Add headers
                            for (int i = 0; i < properties.Length; i++)
                            {
                                ws.Cells[1, i + 1].Value = properties[i].Name;
                                ws.Cells[1, i + 1].Style.Font.Bold = true;
                            }

                            // Add data
                            int row = 0;
                            foreach (var item in enumerableList)
                            {
                                for (int col = 0; col < properties.Length; col++)
                                {
                                    var value = properties[col].GetValue(item);
                                    ws.Cells[row + 2, col + 1].Value = value;
                                }
                                row++;
                            }

                            // Auto-fit columns
                            ws.Cells.AutoFitColumns();
                        }
                    }
                    else
                    {
                        // Handle non-enumerable data (single values)
                        ws.Cells[1, 1].Value = "Data";
                        ws.Cells[1, 1].Style.Font.Bold = true;
                        ws.Cells[2, 1].Value = data?.ToString() ?? "No data";
                    }
                }

                var result = await Task.FromResult(package.GetAsByteArray());
                LogOperationComplete(nameof(GenerateExcelWithMultipleSheetsAsync), new { fileName, sheetCount = worksheets.Count, fileSize = result.Length });
                return result;
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(GenerateExcelWithMultipleSheetsAsync), ex, new { fileName, sheetCount = worksheets.Count });
                throw new ExcelGenerationException($"Failed to generate multi-sheet Excel file: {ex.Message}", ex);
            }
        }
    }
}