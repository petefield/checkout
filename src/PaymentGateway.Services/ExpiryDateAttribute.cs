using PaymentGateway.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Services
{
    public class ExpiryDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var expiryDateValue = value as IExpiryDate;
            if (expiryDateValue == null) return CreateValidationResult("Expiry date is required.", context);

            var expiryDate = new DateTime(expiryDateValue.Year, expiryDateValue.Month, 1).AddMonths(1).AddDays(-1);

            return expiryDate >= DateTime.Now
                ? ValidationResult.Success
                : CreateValidationResult(GetErrorMessage(expiryDateValue), context);
        }

        private ValidationResult CreateValidationResult(string message, ValidationContext context) => new ValidationResult(message, new[] { context.MemberName });

        private string GetErrorMessage(IExpiryDate expiryDate) => $"'{expiryDate}' is in the past.";
    }
}