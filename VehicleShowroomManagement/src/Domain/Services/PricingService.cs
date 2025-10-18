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

            // Calculate final price using operator overloading
            var finalPrice = basePrice + taxAmount;

            return finalPrice;
        }

        public Money CalculateTax(Money basePrice, decimal taxRate)
        {
            return basePrice * (taxRate / 100);
        }

        public bool IsPriceValid(Vehicle vehicle, Money price)
        {
            // Get the vehicle purchase price
            var vehiclePurchasePrice = new Money(vehicle.PurchasePrice);

            // Calculate minimum allowed price based on profit margin
            var minimumPrice = vehiclePurchasePrice * (1 + (_minimumProfitMargin / 100));

            // Price should not be below minimum
            if (price < minimumPrice)
            {
                return false;
            }

            return true;
        }

        public Money CalculateOrderTotal(Order order)
        {
            // Calculate total from sale price (new schema)
            return new Money(order.SalePrice);
        }

        public Money ApplyDiscount(Money price, decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));
            }

            return price.ApplyDiscount(discountPercentage);
        }

        public decimal CalculateProfit(decimal cost, decimal margin)
        {
            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative", nameof(cost));
            
            if (margin < 0)
                throw new ArgumentException("Margin cannot be negative", nameof(margin));
            
            return cost * margin;
        }

        public decimal CalculateSellingPrice(decimal cost, decimal margin)
        {
            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative", nameof(cost));
            
            if (margin < 0)
                throw new ArgumentException("Margin cannot be negative", nameof(margin));
            
            return cost + CalculateProfit(cost, margin);
        }

        public decimal CalculateMargin(decimal cost, decimal sellingPrice)
        {
            if (cost <= 0)
                throw new ArgumentException("Cost must be greater than zero", nameof(cost));
            
            if (sellingPrice < 0)
                throw new ArgumentException("Selling price cannot be negative", nameof(sellingPrice));
            
            return (sellingPrice - cost) / cost;
        }
    }
}