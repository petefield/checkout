using System;

namespace PaymentGateway.Models.Contracts
{
    public interface IPaymentResponse
    {
        Guid Id { get;  }
        IPaymentRequest RequestDetails { get; }
        PaymentStatus Status { get; }
        string Reason {get;}
    }
}
