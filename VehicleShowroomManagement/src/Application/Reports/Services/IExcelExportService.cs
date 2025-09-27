using System.Collections.Generic;
using VehicleShowroomManagement.Application.Reports.DTOs;

namespace VehicleShowroomManagement.Application.Reports.Services
{
    /// <summary>
    /// Interface for Excel export services
    /// </summary>
    public interface IExcelExportService
    {
        byte[] ExportStockAvailabilityReport(IEnumerable<StockAvailabilityReportDto> data);
        byte[] ExportCustomerInfoReport(IEnumerable<CustomerInfoReportDto> data);
        byte[] ExportVehicleMasterReport(IEnumerable<VehicleMasterReportDto> data);
        byte[] ExportAllotmentDetailsReport(IEnumerable<AllotmentDetailsReportDto> data);
        byte[] ExportWaitingListReport(IEnumerable<WaitingListReportDto> data);
    }
}
