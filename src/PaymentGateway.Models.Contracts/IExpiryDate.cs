using System;

namespace PaymentGateway.Models.Contracts
{
    public interface IExpiryDate
    {
        int Year {get; set;}
        int Month { get; set; }
    }
}
