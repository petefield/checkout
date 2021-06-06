using System;

namespace PaymentGateway.Models.Contracts
{
    public interface IPaymentDetails
    {
        Guid Id { get;  }
        string CardNumber { get; }
        int Amount { get; }
        string CVV { get; }
        PaymentStatus Status { get; }
        public string BankResponseId { get; }

    }
}
