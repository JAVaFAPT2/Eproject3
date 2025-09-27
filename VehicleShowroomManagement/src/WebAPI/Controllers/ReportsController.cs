using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Reports.Queries.GetStockAvailabilityReport;
using VehicleShowroomManagement.Application.Features.Reports.Queries.GetCustomerInfoReport;
using VehicleShowroomManagement.Application.Features.Reports.Queries.GetVehicleMasterReport;
using VehicleShowroomManagement.Application.Features.Reports.Queries.GetAllotmentDetailsReport;
using VehicleShowroomManagement.Application.Features.Reports.Queries.GetWaitingListReport;
using VehicleShowroomManagement.Application.Features.Reports.Commands.ExportStockAvailability;
using VehicleShowroomManagement.Application.Features.Reports.Commands.ExportCustomerInfo;
using VehicleShowroomManagement.Application.Features.Reports.Commands.ExportVehicleMaster;
using VehicleShowroomManagement.Application.Features.Reports.Commands.ExportAllotmentDetails;
using VehicleShowroomManagement.Application.Features.Reports.Commands.ExportWaitingList;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for reporting operations
    /// </summary>
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets stock availability report
        /// </summary>
        [HttpGet("stock-availability")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetStockAvailabilityReport(
            [FromQuery] string? brand = null,
            [FromQuery] string? model = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetStockAvailabilityReportQuery(brand, model, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets customer information report
        /// </summary>
        [HttpGet("customer-info")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetCustomerInfoReport(
            [FromQuery] string? searchTerm = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetCustomerInfoReportQuery(searchTerm, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets vehicle master report
        /// </summary>
        [HttpGet("vehicle-master")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetVehicleMasterReport(
            [FromQuery] string? brand = null,
            [FromQuery] string? model = null,
            [FromQuery] int? year = null)
        {
            var query = new GetVehicleMasterReportQuery(brand, model, year);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets allotment details report
        /// </summary>
        [HttpGet("allotment-details")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetAllotmentDetailsReport(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? customerId = null)
        {
            var query = new GetAllotmentDetailsReportQuery(fromDate, toDate, customerId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets waiting list report
        /// </summary>
        [HttpGet("waiting-list")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetWaitingListReport(
            [FromQuery] string? modelId = null,
            [FromQuery] string? customerId = null)
        {
            var query = new GetWaitingListReportQuery(modelId, customerId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Exports stock availability report to Excel
        /// </summary>
        [HttpGet("export/stock-availability")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportStockAvailability(
            [FromQuery] string? brand = null,
            [FromQuery] string? model = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var command = new ExportStockAvailabilityCommand(brand, model, fromDate, toDate);
            var result = await _mediator.Send(command);

            return File(result.Content, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Exports customer info report to Excel
        /// </summary>
        [HttpGet("export/customer-info")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportCustomerInfo(
            [FromQuery] string? searchTerm = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var command = new ExportCustomerInfoCommand(searchTerm, fromDate, toDate);
            var result = await _mediator.Send(command);

            return File(result.Content, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Exports vehicle master report to Excel
        /// </summary>
        [HttpGet("export/vehicle-master")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportVehicleMaster(
            [FromQuery] string? brand = null,
            [FromQuery] string? model = null,
            [FromQuery] int? year = null)
        {
            var command = new ExportVehicleMasterCommand(brand, model, year);
            var result = await _mediator.Send(command);

            return File(result.Content, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Exports allotment details report to Excel
        /// </summary>
        [HttpGet("export/allotment-details")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportAllotmentDetails(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? customerId = null)
        {
            var command = new ExportAllotmentDetailsCommand(fromDate, toDate, customerId);
            var result = await _mediator.Send(command);

            return File(result.Content, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Exports waiting list report to Excel
        /// </summary>
        [HttpGet("export/waiting-list")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ExportWaitingList(
            [FromQuery] string? modelId = null,
            [FromQuery] string? customerId = null)
        {
            var command = new ExportWaitingListCommand(modelId, customerId);
            var result = await _mediator.Send(command);

            return File(result.Content, result.ContentType, result.FileName);
        }
    }
}