using System;
using System.Threading.Tasks;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Domain.Services
{
    /// <summary>
    /// Implementation of pricing domain service
    /// </summary>
    public class PricingService : IPricingService
    {
        private readonly decimal _defaultTaxRate = 8.5m; // 8.5% default tax rate
        private readonly decimal _minimumProfitMargin = 5.0m; // 5% minimum profit margin

        public async Task<Money> CalculateFinalPrice(Vehicle vehicle, decimal discountPercentage = 0)
        {
            var basePrice = new Money(vehicle.Price);

            // Apply discount if specified
            if (discountPercentage > 0)
            {
                basePrice = ApplyDiscount(basePrice, discountPercentage);
            }

            // Calculate tax
            var taxAmount = CalculateTax(basePrice, _defaultTaxRate);

            // Calculate final price
            var finalPrice = basePrice.Add(taxAmount);

            // Validate the final price
            var isValid = await IsPriceValid(vehicle, finalPrice);
            if (!isValid)
            {
                throw new InvalidOperationException($"Calculated price is not valid for vehicle {vehicle.VehicleId}");
            }

            return finalPrice;
        }

        public Money CalculateTax(Money basePrice, decimal taxRate)
        {
            return basePrice.Multiply(taxRate / 100);
        }

        public async Task<bool> IsPriceValid(Vehicle vehicle, Money price)
        {
            // Get the model base price (this would typically come from a repository)
            var modelBasePrice = new Money(vehicle.Price);

            // Calculate minimum allowed price based on profit margin
            var minimumPrice = modelBasePrice.Multiply(1 + (_minimumProfitMargin / 100));

            // Price should not be below minimum
            if (price.Amount < minimumPrice.Amount)
            {
                return false;
            }

            return true;
        }

        public async Task<Money> CalculateOrderTotal(SalesOrder salesOrder)
        {
            Money total = new Money(0);

            foreach (var item in salesOrder.SalesOrderItems)
            {
                var itemTotal = new Money(item.UnitPrice * item.Quantity);
                itemTotal = ApplyDiscount(itemTotal, (item.Discount / itemTotal.Amount) * 100);
                total = total.Add(itemTotal);
            }

            return total;
        }

        public Money ApplyDiscount(Money price, decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));
            }

            return price.ApplyDiscount(discountPercentage);
        }
    }
}