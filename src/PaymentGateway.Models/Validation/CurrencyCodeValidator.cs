using System.Collections.Generic;

namespace PaymentGateway.Models.Validation
{
    public class ValidCurrencyCodeProvider : IValidCurrencyCodeProvider
    {
        private readonly string[] _validCurrencies = new[] { "GBP", "USD" };
        public IEnumerable<string> ValidCurrencyCodes => _validCurrencies;
    }
}
