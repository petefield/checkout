using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Models.Validation
{
    public class CurrencyCodeAttribute : ValidationAttribute
    {

        public override bool RequiresValidationContext => true;


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string currencyCode = value as string;

            if (string.IsNullOrWhiteSpace(currencyCode)) return CreateValidationResult("A valid Currency Code is required.", validationContext);
            if (currencyCode.Length != 3) return CreateValidationResult("Currency Code must be 3 characters", validationContext);

            var validCurrencyCodeProvider = validationContext.GetService<IValidCurrencyCodeProvider>();

            return validCurrencyCodeProvider.ValidCurrencyCodes.Contains(currencyCode) 
                ? ValidationResult.Success 
                : CreateValidationResult($"'{currencyCode}' is not a currently supported country code", validationContext);
        }

        private ValidationResult CreateValidationResult(string message, ValidationContext context) => new ValidationResult(message, new[] { context.MemberName });
    }
}