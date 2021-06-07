using System;

namespace AcquiringBank.Contracts
{
    public interface IPaymentResponse
    {
        string Id { get; }
        Outcome Outcome { get; }
        string Reason { get; }
        DateTime TimeStamp { get; }
    }
}
