using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CurrencyCodeAttribute : ValidationAttribute
    {

        public override bool RequiresValidationContext => true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string currencyCode) return CreateValidationResult($"'{value}' is not a valid currency code", validationContext);

            var validCurrencyCodeProvider = validationContext.GetService<IValidCurrencyCodeProvider>();
            if (validCurrencyCodeProvider is null) 
                throw new System.InvalidOperationException(" Unable to resolve service for type 'PaymentGateway.Models.Validation.IValidCurrencyCodeProvider'");

            return validCurrencyCodeProvider.ValidCurrencyCodes.Contains(currencyCode) 
                ? ValidationResult.Success 
                : CreateValidationResult($"'{currencyCode}' is not a currently supported country code", validationContext);
        }

        private ValidationResult CreateValidationResult(string message, ValidationContext context) => new ValidationResult(message, new[] { context.MemberName });
    
  

    }
}