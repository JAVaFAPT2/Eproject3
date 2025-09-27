using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Reports.Queries;
using VehicleShowroomManagement.Application.Reports.Services;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// Controller for generating reports
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IExcelExportService _excelExportService;

        public ReportsController(IMediator mediator, IExcelExportService excelExportService)
        {
            _mediator = mediator;
            _excelExportService = excelExportService;
        }

        /// <summary>
        /// Get stock availability report
        /// </summary>
        [HttpGet("stock-availability")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetStockAvailabilityReport(
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] string? status,
            [FromQuery] DateTime? asOfDate)
        {
            var query = new GetStockAvailabilityReportQuery
            {
                Brand = brand,
                Model = model,
                Status = status,
                AsOfDate = asOfDate
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get customer information report
        /// </summary>
        [HttpGet("customer-info")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetCustomerInfoReport(
            [FromQuery] string? searchTerm,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? city,
            [FromQuery] string? state,
            [FromQuery] bool includeOrderHistory = true)
        {
            var query = new GetCustomerInfoReportQuery
            {
                SearchTerm = searchTerm,
                FromDate = fromDate,
                ToDate = toDate,
                City = city,
                State = state,
                IncludeOrderHistory = includeOrderHistory
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get vehicle master information report
        /// </summary>
        [HttpGet("vehicle-master")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetVehicleMasterReport(
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] string? status,
            [FromQuery] int? year,
            [FromQuery] string? color,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] bool includeRegistrationInfo = true,
            [FromQuery] bool includeServiceHistory = true)
        {
            var query = new GetVehicleMasterReportQuery
            {
                Brand = brand,
                Model = model,
                Status = status,
                Year = year,
                Color = color,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                FromDate = fromDate,
                ToDate = toDate,
                IncludeRegistrationInfo = includeRegistrationInfo,
                IncludeServiceHistory = includeServiceHistory
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get allotment details report
        /// </summary>
        [HttpGet("allotment-details")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetAllotmentDetailsReport(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? allotmentType,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] bool includeExpired = true,
            [FromQuery] bool includeConverted = true)
        {
            var query = new GetAllotmentDetailsReportQuery
            {
                SearchTerm = searchTerm,
                Status = status,
                AllotmentType = allotmentType,
                FromDate = fromDate,
                ToDate = toDate,
                IncludeExpired = includeExpired,
                IncludeConverted = includeConverted
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get waiting list details report
        /// </summary>
        [HttpGet("waiting-list")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetWaitingListReport(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] string? vehicleType,
            [FromQuery] int? priority,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] bool includeExpired = true,
            [FromQuery] bool includeConverted = true)
        {
            var query = new GetWaitingListReportQuery
            {
                SearchTerm = searchTerm,
                Status = status,
                Brand = brand,
                Model = model,
                VehicleType = vehicleType,
                Priority = priority,
                FromDate = fromDate,
                ToDate = toDate,
                IncludeExpired = includeExpired,
                IncludeConverted = includeConverted
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Export stock availability report to Excel
        /// </summary>
        [HttpGet("stock-availability/export")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportStockAvailabilityReport(
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] string? status,
            [FromQuery] DateTime? asOfDate)
        {
            var query = new GetStockAvailabilityReportQuery
            {
                Brand = brand,
                Model = model,
                Status = status,
                AsOfDate = asOfDate
            };

            var result = await _mediator.Send(query);
            var excelData = _excelExportService.ExportStockAvailabilityReport(new[] { result });
            
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"StockAvailabilityReport_{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        /// <summary>
        /// Export customer information report to Excel
        /// </summary>
        [HttpGet("customer-info/export")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportCustomerInfoReport(
            [FromQuery] string? searchTerm,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? city,
            [FromQuery] string? state,
            [FromQuery] bool includeOrderHistory = true)
        {
            var query = new GetCustomerInfoReportQuery
            {
                SearchTerm = searchTerm,
                FromDate = fromDate,
                ToDate = toDate,
                City = city,
                State = state,
                IncludeOrderHistory = includeOrderHistory
            };

            var result = await _mediator.Send(query);
            var excelData = _excelExportService.ExportCustomerInfoReport(new[] { result });
            
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"CustomerInfoReport_{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        /// <summary>
        /// Export vehicle master report to Excel
        /// </summary>
        [HttpGet("vehicle-master/export")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportVehicleMasterReport(
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] string? status,
            [FromQuery] int? year,
            [FromQuery] string? color,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] bool includeRegistrationInfo = true,
            [FromQuery] bool includeServiceHistory = true)
        {
            var query = new GetVehicleMasterReportQuery
            {
                Brand = brand,
                Model = model,
                Status = status,
                Year = year,
                Color = color,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                FromDate = fromDate,
                ToDate = toDate,
                IncludeRegistrationInfo = includeRegistrationInfo,
                IncludeServiceHistory = includeServiceHistory
            };

            var result = await _mediator.Send(query);
            var excelData = _excelExportService.ExportVehicleMasterReport(new[] { result });
            
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"VehicleMasterReport_{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        /// <summary>
        /// Export allotment details report to Excel
        /// </summary>
        [HttpGet("allotment-details/export")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportAllotmentDetailsReport(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? allotmentType,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] bool includeExpired = true,
            [FromQuery] bool includeConverted = true)
        {
            var query = new GetAllotmentDetailsReportQuery
            {
                SearchTerm = searchTerm,
                Status = status,
                AllotmentType = allotmentType,
                FromDate = fromDate,
                ToDate = toDate,
                IncludeExpired = includeExpired,
                IncludeConverted = includeConverted
            };

            var result = await _mediator.Send(query);
            var excelData = _excelExportService.ExportAllotmentDetailsReport(new[] { result });
            
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"AllotmentDetailsReport_{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        /// <summary>
        /// Export waiting list report to Excel
        /// </summary>
        [HttpGet("waiting-list/export")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportWaitingListReport(
            [FromQuery] string? searchTerm,
            [FromQuery] string? status,
            [FromQuery] string? brand,
            [FromQuery] string? model,
            [FromQuery] string? vehicleType,
            [FromQuery] int? priority,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] bool includeExpired = true,
            [FromQuery] bool includeConverted = true)
        {
            var query = new GetWaitingListReportQuery
            {
                SearchTerm = searchTerm,
                Status = status,
                Brand = brand,
                Model = model,
                VehicleType = vehicleType,
                Priority = priority,
                FromDate = fromDate,
                ToDate = toDate,
                IncludeExpired = includeExpired,
                IncludeConverted = includeConverted
            };

            var result = await _mediator.Send(query);
            var excelData = _excelExportService.ExportWaitingListReport(new[] { result });
            
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"WaitingListReport_{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }
    }
}
