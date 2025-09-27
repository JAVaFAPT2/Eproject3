using System.Globalization;

namespace VehicleShowroomManagement.Domain.ValueObjects
{
    /// <summary>
    /// Money value object with validation and operations
    /// </summary>
    public record Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency = "USD")
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

            if (currency.Length != 3)
                throw new ArgumentException("Currency must be a 3-letter code", nameof(currency));

            Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
            Currency = currency.ToUpperInvariant();
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

        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money.Amount * multiplier, money.Currency);
        }

        public static Money operator *(decimal multiplier, Money money)
        {
            return money * multiplier;
        }

        public static Money operator /(Money money, decimal divisor)
        {
            if (divisor == 0)
                throw new DivideByZeroException("Cannot divide by zero");

            return new Money(money.Amount / divisor, money.Currency);
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

        public static implicit operator decimal(Money money) => money.Amount;

        public override string ToString()
        {
            return $"{Amount:C} {Currency}";
        }

        public string ToString(string format)
        {
            return Amount.ToString(format, CultureInfo.InvariantCulture);
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