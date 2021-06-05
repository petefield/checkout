using System;
using PaymentGateway.Models.Contracts;
using PaymentGateway.Services;

namespace PaymentGateway.Models
{
    public class PaymentResponse : IPaymentResponse
    {
        public Guid Id { get; set; }

        public IPaymentRequest RequestDetails { get; set; }

        public PaymentResponseStatus Status { get; set; }

        public string Reason { get; set; }
    }
}
