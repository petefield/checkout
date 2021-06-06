using PaymentGateway.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models.Validation
{
    public class ExpiryDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is not IExpiryDate expiryDateValue) return CreateValidationResult("Expiry date is required.", context);

            if (!TryParseExpiryDate(expiryDateValue, out var expiryDate)) return CreateValidationResult("Expiry date is not valid.", context);

            return expiryDate < DateTime.Now ? CreateValidationResult(GetErrorMessage(expiryDateValue), context) : ValidationResult.Success;

        }

        private bool TryParseExpiryDate(IExpiryDate expiryDateValue, out DateTime expiryDate) {
            try
            {
                expiryDate = new DateTime(expiryDateValue.Year, expiryDateValue.Month, 1).AddMonths(1).AddDays(-1);
                return true;
            }
            catch (Exception)
            {
                expiryDate = DateTime.MinValue;
                return false;
            }
        }

        private static ValidationResult CreateValidationResult(string message, ValidationContext context) => new ValidationResult(message, new[] { context.MemberName });

        private static string GetErrorMessage(IExpiryDate expiryDate) => $"'{expiryDate}' is in the past.";
    }
}