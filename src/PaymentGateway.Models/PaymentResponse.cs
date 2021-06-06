using System;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Models
{
    public class PaymentResponse : IPaymentResponse
    {
        public Guid Id { get; set; }

        public IPaymentRequest RequestDetails { get; set; }

        public PaymentStatus Status { get; set; }

        public string Reason { get; set; }
    }
}
