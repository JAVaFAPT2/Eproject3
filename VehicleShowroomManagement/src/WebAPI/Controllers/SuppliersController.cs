using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Suppliers.Commands.CreateSupplier;
using VehicleShowroomManagement.Application.Features.Suppliers.Commands.UpdateSupplier;
using VehicleShowroomManagement.Application.Features.Suppliers.Commands.DeleteSupplier;
using VehicleShowroomManagement.Application.Features.Suppliers.Queries.GetSupplierById;
using VehicleShowroomManagement.Application.Features.Suppliers.Queries.GetSuppliers;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for supplier management
    /// </summary>
    [ApiController]
    [Route("api/suppliers")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuppliersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all suppliers with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetSuppliers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? country = null,
            [FromQuery] bool? isActive = null)
        {
            var query = new GetSuppliersQuery(pageNumber, pageSize, searchTerm, country, isActive);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a supplier by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetSupplier(string id)
        {
            var query = new GetSupplierByIdQuery(id);
            var supplier = await _mediator.Send(query);

            if (supplier == null)
                return NotFound(new { message = "Supplier not found" });

            return Ok(supplier);
        }

        /// <summary>
        /// Creates a new supplier
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequest request)
        {
            var command = new CreateSupplierCommand(
                request.Name,
                request.ContactPerson,
                request.Email,
                request.Phone,
                request.Address,
                request.TaxNumber,
                request.PaymentTerms);

            var supplierId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetSupplier), new { id = supplierId }, 
                new { id = supplierId, message = "Supplier created successfully" });
        }

        /// <summary>
        /// Updates a supplier
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSupplier(string id, [FromBody] UpdateSupplierRequest request)
        {
            var command = new UpdateSupplierCommand(
                id,
                request.Name,
                request.ContactPerson,
                request.Email,
                request.Phone,
                request.Address,
                request.TaxNumber,
                request.PaymentTerms,
                request.IsActive);

            await _mediator.Send(command);

            return Ok(new { message = "Supplier updated successfully" });
        }

        /// <summary>
        /// Deactivates a supplier
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSupplier(string id)
        {
            var command = new DeleteSupplierCommand(id);
            await _mediator.Send(command);

            return Ok(new { message = "Supplier deactivated successfully" });
        }
    }

    /// <summary>
    /// Request model for creating supplier
    /// </summary>
    public class CreateSupplierRequest
    {
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? TaxNumber { get; set; }
        public int PaymentTerms { get; set; } = 30; // Days
    }

    /// <summary>
    /// Request model for updating supplier
    /// </summary>
    public class UpdateSupplierRequest
    {
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? TaxNumber { get; set; }
        public int PaymentTerms { get; set; } = 30; // Days
        public bool IsActive { get; set; } = true;
    }
}