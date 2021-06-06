using System;
using System.Collections.Generic;

namespace PaymentGateway.Models.Validation
{
    public interface IValidCurrencyCodeProvider
    {
        IEnumerable<string> ValidCurrencyCodes { get;  }
    }
}
