using AcquiringBank.Contracts;
using System;

namespace PaymentGateway.Models.Contracts
{
    public interface IPaymentDetails
    {
        Guid Id { get;  }
        string CardNumber { get; }
        int Amount { get; }
        string CurrencyCode { get;  }
        string CVV { get; }
        Outcome Outcome { get; }
        string Reason { get;  }
        string BankReference { get; }
        DateTime Received { get;  }
        DateTime Processed { get;  }
    }
}
