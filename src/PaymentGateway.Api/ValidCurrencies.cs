using PaymentGateway.Services.Contracts.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api
{
    public class ValidCurrencies : IValidCurrencyCodeProvider
    {
        public IEnumerable<string> ValidCurrencyCodes => new[] { "GBP", "USD"};
    }
}
