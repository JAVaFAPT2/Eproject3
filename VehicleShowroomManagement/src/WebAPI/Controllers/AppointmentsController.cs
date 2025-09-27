using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleShowroomManagement.Application.Features.Appointments.Commands.CreateAppointment;
using VehicleShowroomManagement.Application.Features.Appointments.Commands.ConfirmAppointment;
using VehicleShowroomManagement.Application.Features.Appointments.Commands.CancelAppointment;
using VehicleShowroomManagement.Application.Features.Appointments.Commands.RescheduleAppointment;
using VehicleShowroomManagement.Application.Features.Appointments.Commands.CompleteAppointment;
using VehicleShowroomManagement.Application.Features.Appointments.Commands.StartAppointment;
using VehicleShowroomManagement.Application.Features.Appointments.Commands.UpdateAppointmentRequirements;
using VehicleShowroomManagement.Application.Features.Appointments.Queries.GetAppointments;
using VehicleShowroomManagement.Application.Features.Appointments.Queries.GetAppointmentById;
using VehicleShowroomManagement.Application.Features.Appointments.Queries.GetDealerAvailability;
using VehicleShowroomManagement.Application.Features.Appointments.Queries.GetCustomerAppointments;
using VehicleShowroomManagement.WebAPI.Models.Appointments;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Controllers
{
    /// <summary>
    /// API Controller for appointment booking and management
    /// </summary>
    [ApiController]
    [Route("api/appointments")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all appointments with pagination and filters
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetAppointments(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] AppointmentStatus? status = null,
            [FromQuery] string? dealerId = null,
            [FromQuery] string? customerId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] AppointmentType? type = null)
        {
            var query = new GetAppointmentsQuery(
                pageNumber, pageSize, status, dealerId, customerId, fromDate, toDate, type);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets an appointment by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(string id)
        {
            var query = new GetAppointmentByIdQuery(id);
            var appointment = await _mediator.Send(query);

            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            return Ok(appointment);
        }

        /// <summary>
        /// Gets customer's appointments
        /// </summary>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerAppointments(
            string customerId,
            [FromQuery] AppointmentStatus? status = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetCustomerAppointmentsQuery(customerId, status, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets dealer availability for appointment booking
        /// </summary>
        [HttpGet("availability/{dealerId}")]
        public async Task<IActionResult> GetDealerAvailability(
            string dealerId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetDealerAvailabilityQuery(
                dealerId, 
                fromDate ?? DateTime.UtcNow.Date, 
                toDate ?? DateTime.UtcNow.Date.AddDays(30));
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new appointment
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var command = new CreateAppointmentCommand(
                request.CustomerId,
                request.DealerId,
                request.AppointmentDate,
                request.Type,
                request.DurationMinutes,
                request.CustomerRequirements,
                request.VehicleInterest,
                request.BudgetRange);

            var appointmentId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetAppointment), new { id = appointmentId }, 
                new { id = appointmentId, message = "Appointment created successfully" });
        }

        /// <summary>
        /// Confirms an appointment (dealer action)
        /// </summary>
        [HttpPut("{id}/confirm")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> ConfirmAppointment(string id, [FromBody] ConfirmAppointmentRequest request)
        {
            var command = new ConfirmAppointmentCommand(id, request.DealerNotes);
            await _mediator.Send(command);

            return Ok(new { message = "Appointment confirmed successfully" });
        }

        /// <summary>
        /// Cancels an appointment
        /// </summary>
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(string id, [FromBody] CancelAppointmentRequest request)
        {
            var command = new CancelAppointmentCommand(id, request.Reason);
            await _mediator.Send(command);

            return Ok(new { message = "Appointment cancelled successfully" });
        }

        /// <summary>
        /// Reschedules an appointment
        /// </summary>
        [HttpPut("{id}/reschedule")]
        public async Task<IActionResult> RescheduleAppointment(string id, [FromBody] RescheduleAppointmentRequest request)
        {
            var command = new RescheduleAppointmentCommand(id, request.NewAppointmentDate);
            await _mediator.Send(command);

            return Ok(new { message = "Appointment rescheduled successfully" });
        }

        /// <summary>
        /// Completes an appointment (dealer action)
        /// </summary>
        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> CompleteAppointment(string id, [FromBody] CompleteAppointmentRequest request)
        {
            var command = new CompleteAppointmentCommand(
                id, 
                request.DealerNotes, 
                request.FollowUpRequired, 
                request.FollowUpDate);
            await _mediator.Send(command);

            return Ok(new { message = "Appointment completed successfully" });
        }

        /// <summary>
        /// Starts an appointment (dealer action)
        /// </summary>
        [HttpPut("{id}/start")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> StartAppointment(string id)
        {
            var command = new StartAppointmentCommand(id);
            await _mediator.Send(command);

            return Ok(new { message = "Appointment started successfully" });
        }

        /// <summary>
        /// Updates customer requirements for an appointment
        /// </summary>
        [HttpPut("{id}/requirements")]
        public async Task<IActionResult> UpdateRequirements(string id, [FromBody] UpdateRequirementsRequest request)
        {
            var command = new UpdateAppointmentRequirementsCommand(
                id, 
                request.CustomerRequirements, 
                request.VehicleInterest, 
                request.BudgetRange);
            await _mediator.Send(command);

            return Ok(new { message = "Requirements updated successfully" });
        }
    }
}