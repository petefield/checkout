using PaymentGateway.Services.Contracts.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PaymentGateway.Services
{
    public class CurrencyCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            string currencyCode = value as string;
            if (string.IsNullOrWhiteSpace(currencyCode)) return CreateValidationResult("Currency Code is required.", context);
            if (currencyCode.Length != 3) return CreateValidationResult("Currency Code must be 3 characters", context);

            var validCurrencyCodeProvider = GetService<IValidCurrencyCodeProvider>(context);

            return validCurrencyCodeProvider.ValidCurrencyCodes.Contains(currencyCode) 
                ? ValidationResult.Success 
                : CreateValidationResult(GetErrorMessage(currencyCode),context);
        }

        private ValidationResult CreateValidationResult(string message, ValidationContext context) => new ValidationResult(message, new[] { context.MemberName });

        private string GetErrorMessage(string currencyCode) => $"'{currencyCode}' is not a currently supported country code";

        private T GetService<T>(ValidationContext context) where T : class { 
            T result =  context.GetService(typeof(T)) as T;
            if (result == null) throw new System.Exception($"No instance of {typeof(T).Name} could be created.");
            return result;
        }
    }
}