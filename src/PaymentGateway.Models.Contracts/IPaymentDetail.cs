using System;

namespace PaymentGateway.Models.Contracts
{
    public interface IPaymentDetail
    {
        Guid Id { get; set; }
        string CardNumber { get; set; }
        int Amount { get; set;  }
        string CVV { get; set;  }
        PaymentResponseStatus Status { get; set; } 
    }
}
