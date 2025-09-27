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

        public Money CalculateFinalPrice(Vehicle vehicle, decimal discountPercentage = 0)
        {
            var basePrice = new Money(vehicle.PurchasePrice);

            // Apply discount if specified
            if (discountPercentage > 0)
            {
                basePrice = ApplyDiscount(basePrice, discountPercentage);
            }

            // Calculate tax
            var taxAmount = CalculateTax(basePrice, _defaultTaxRate);

            // Calculate final price
            var finalPrice = basePrice.Add(taxAmount);

            return finalPrice;
        }

        public Money CalculateTax(Money basePrice, decimal taxRate)
        {
            return basePrice.Multiply(taxRate / 100);
        }

        public bool IsPriceValid(Vehicle vehicle, Money price)
        {
            // Get the vehicle purchase price
            var vehiclePurchasePrice = new Money(vehicle.PurchasePrice);

            // Calculate minimum allowed price based on profit margin
            var minimumPrice = vehiclePurchasePrice.Multiply(1 + (_minimumProfitMargin / 100));

            // Price should not be below minimum
            if (price.Amount < minimumPrice.Amount)
            {
                return false;
            }

            return true;
        }

        public Money CalculateOrderTotal(SalesOrder salesOrder)
        {
            // Calculate total from sale price (single vehicle per order in new schema)
            return new Money(salesOrder.SalePrice);
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