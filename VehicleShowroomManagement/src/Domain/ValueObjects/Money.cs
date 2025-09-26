using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleShowroomManagement.Domain.ValueObjects
{
    /// <summary>
    /// Money value object representing monetary values
    /// </summary>
    [ComplexType]
    public class Money : IEquatable<Money>
    {
        [Column(TypeName = "decimal(12,2)")]
        public decimal Amount { get; private set; }

        [Column(TypeName = "nvarchar(3)")]
        public string Currency { get; private set; } = "USD";

        // Required for Entity Framework
        protected Money() { }

        public Money(decimal amount, string currency = "USD")
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));

            Amount = Math.Round(amount, 2);
            Currency = currency.ToUpper();
        }

        public override bool Equals(object? obj)
        {
            return obj is Money money && Equals(money);
        }

        public bool Equals(Money? other)
        {
            if (other is null)
                return false;

            return Amount == other.Amount && Currency == other.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        public static bool operator ==(Money? left, Money? right)
        {
            return EqualityComparer<Money>.Default.Equals(left, right);
        }

        public static bool operator !=(Money? left, Money? right)
        {
            return !(left == right);
        }

        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot add money with different currencies");

            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot subtract money with different currencies");

            return new Money(left.Amount - right.Amount, left.Currency);
        }

        public static bool operator >(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot compare money with different currencies");

            return left.Amount > right.Amount;
        }

        public static bool operator <(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Cannot compare money with different currencies");

            return left.Amount < right.Amount;
        }

        public override string ToString()
        {
            return $"{Amount:F2} {Currency}";
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot add money with different currencies");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot subtract money with different currencies");

            return new Money(Amount - other.Amount, Currency);
        }

        public Money Multiply(decimal factor)
        {
            return new Money(Amount * factor, Currency);
        }

        public Money ApplyDiscount(decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));

            var discountAmount = Amount * (discountPercentage / 100);
            return new Money(Amount - discountAmount, Currency);
        }
    }
}