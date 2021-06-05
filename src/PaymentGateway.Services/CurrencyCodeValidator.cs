using PaymentGateway.Services.Contracts.Validation;
using System.Collections.Generic;

namespace PaymentGateway.Services
{
    public class ValidCurrencyCodeProvider : IValidCurrencyCodeProvider
    {
        private readonly string[] _validCurrencies = new[] { "GBP", "USD" };
        public IEnumerable<string> ValidCurrencyCodes => _validCurrencies;
    }
}
