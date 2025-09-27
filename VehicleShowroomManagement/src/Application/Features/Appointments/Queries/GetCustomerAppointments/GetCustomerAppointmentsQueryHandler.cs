using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Queries.GetCustomerAppointments
{
    public class GetCustomerAppointmentsQueryHandler : IRequestHandler<GetCustomerAppointmentsQuery, object>
    {
        public async Task<object> Handle(GetCustomerAppointmentsQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new { 
                customerId = request.CustomerId, 
                appointments = new object[0],
                message = "Customer appointments ready" 
            };
        }
    }
}