using System;
using System.Collections.Generic;

namespace PaymentGateway.Services.Contracts.Validation
{
    public interface IValidCurrencyCodeProvider
    {
        IEnumerable<string> ValidCurrencyCodes { get;  }
    }
}
