using System;

namespace PaymentGateway.Models.Contracts
{
    public interface IPaymentRequest
    {
        Guid RequestId { get; set; }
        string CardNumber { get; set; }
        string CVV {get; set;}
        IExpiryDate ExpiryDate { get; set; }
        int Amount {get; set; }
        string CurrencyCode{get; set;}
        DateTime TimeStamp { get; set;  }
    }
}
