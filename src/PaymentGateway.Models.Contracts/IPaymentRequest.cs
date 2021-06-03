using System;

namespace PaymentGateway.Models.Contracts
{
    public interface IPaymentRequest
    {
        string CardNumber { get; set; }
        string CVV {get; set;}
        IExpiryDate ExpiryDate { get; set; }
        decimal Amount {get; set; }
        string CurencyCode{get; set;}
    }
}
