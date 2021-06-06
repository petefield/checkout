using PaymentGateway.Models.Validation;
using System.Collections.Generic;

namespace PaymentGateway.Api
{
    public class ValidCurrencies : IValidCurrencyCodeProvider
    {
        public IEnumerable<string> ValidCurrencyCodes => new[] { "GBP", "USD"};
    }
}
