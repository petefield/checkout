using System;

namespace PaymentGateway.Models.Contracts
{
    public interface IPaymentResponse
    {
        Guid Id { get;  }
        IPaymentRequest RequestDetails { get; }
        PaymentResponseStatus Status { get; }
        string Reason {get;}
    }
}
