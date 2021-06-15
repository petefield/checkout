using System;
using System.Collections.Generic;

namespace PaymentGateway.Validation
{
    public interface IValidCurrencyCodeProvider    {
        IEnumerable<string> ValidCurrencyCodes { get;  }
    }
}
