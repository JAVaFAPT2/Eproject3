using System.Threading.Tasks;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Services
{
    /// <summary>
    /// Domain service for pricing calculations and validations
    /// </summary>
    public interface IPricingService : IDomainService
    {
        /// <summary>
        /// Calculates the final price for a vehicle including taxes and discounts
        /// </summary>
        Money CalculateFinalPrice(Vehicle vehicle, decimal discountPercentage = 0);

        /// <summary>
        /// Calculates tax amount based on vehicle price and tax rate
        /// </summary>
        Money CalculateTax(Money basePrice, decimal taxRate);

        /// <summary>
        /// Validates if a price is within acceptable range for a vehicle model
        /// </summary>
        bool IsPriceValid(Vehicle vehicle, Money price);

        /// <summary>
        /// Calculates total price for a sales order
        /// </summary>
        Money CalculateOrderTotal(SalesOrder salesOrder);

        /// <summary>
        /// Applies discount to a price
        /// </summary>
        Money ApplyDiscount(Money price, decimal discountPercentage);
    }
}