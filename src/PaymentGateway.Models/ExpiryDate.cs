using System;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Models
{
    public class ExpiryDate : IExpiryDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
