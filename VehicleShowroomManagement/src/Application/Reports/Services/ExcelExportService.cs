using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using VehicleShowroomManagement.Application.Reports.DTOs;

namespace VehicleShowroomManagement.Application.Reports.Services
{
    /// <summary>
    /// Excel export service implementation
    /// </summary>
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportStockAvailabilityReport(IEnumerable<StockAvailabilityReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Stock Availability");
            
            // Headers
            worksheet.Cell(1, 1).Value = "Brand";
            worksheet.Cell(1, 2).Value = "Model";
            worksheet.Cell(1, 3).Value = "VIN";
            worksheet.Cell(1, 4).Value = "Color";
            worksheet.Cell(1, 5).Value = "Year";
            worksheet.Cell(1, 6).Value = "Price";
            worksheet.Cell(1, 7).Value = "Status";
            worksheet.Cell(1, 8).Value = "Last Updated";

            // Data
            worksheet.Cell(2, 1).InsertData(data);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] ExportCustomerInfoReport(IEnumerable<CustomerInfoReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Customer Information");

            // Headers
            worksheet.Cell(1, 1).Value = "Customer Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Phone";
            worksheet.Cell(1, 4).Value = "Address";
            worksheet.Cell(1, 5).Value = "City";
            worksheet.Cell(1, 6).Value = "State";
            worksheet.Cell(1, 7).Value = "Total Orders";
            worksheet.Cell(1, 8).Value = "Total Spent";

            // Data
            worksheet.Cell(2, 1).InsertData(data);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] ExportVehicleMasterReport(IEnumerable<VehicleMasterReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Vehicle Master");

            // Headers
            worksheet.Cell(1, 1).Value = "VIN";
            worksheet.Cell(1, 2).Value = "Brand";
            worksheet.Cell(1, 3).Value = "Model";
            worksheet.Cell(1, 4).Value = "Year";
            worksheet.Cell(1, 5).Value = "Color";
            worksheet.Cell(1, 6).Value = "Price";
            worksheet.Cell(1, 7).Value = "Mileage";
            worksheet.Cell(1, 8).Value = "Status";
            worksheet.Cell(1, 9).Value = "Registration Number";
            worksheet.Cell(1, 10).Value = "Service Count";
            worksheet.Cell(1, 11).Value = "Last Service Date";

            // Data
            worksheet.Cell(2, 1).InsertData(data);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] ExportAllotmentDetailsReport(IEnumerable<AllotmentDetailsReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Allotment Details");

            // Headers
            worksheet.Cell(1, 1).Value = "Allotment Number";
            worksheet.Cell(1, 2).Value = "Vehicle VIN";
            worksheet.Cell(1, 3).Value = "Customer Name";
            worksheet.Cell(1, 4).Value = "Allotment Date";
            worksheet.Cell(1, 5).Value = "Expiry Date";
            worksheet.Cell(1, 6).Value = "Status";
            worksheet.Cell(1, 7).Value = "Allotment Type";
            worksheet.Cell(1, 8).Value = "Reservation Amount";

            // Data
            worksheet.Cell(2, 1).InsertData(data);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] ExportWaitingListReport(IEnumerable<WaitingListReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Waiting List");

            // Headers
            worksheet.Cell(1, 1).Value = "Request Number";
            worksheet.Cell(1, 2).Value = "Customer Name";
            worksheet.Cell(1, 3).Value = "Requested Model";
            worksheet.Cell(1, 4).Value = "Requested Brand";
            worksheet.Cell(1, 5).Value = "Preferred Color";
            worksheet.Cell(1, 6).Value = "Min Price";
            worksheet.Cell(1, 7).Value = "Max Price";
            worksheet.Cell(1, 8).Value = "Request Date";
            worksheet.Cell(1, 9).Value = "Priority";
            worksheet.Cell(1, 10).Value = "Status";

            // Data
            worksheet.Cell(2, 1).InsertData(data);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}