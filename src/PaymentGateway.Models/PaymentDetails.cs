using PaymentGateway.Models.Contracts;
using System;


namespace PaymentGateway.Models
{
    public class PaymentDetails : IPaymentDetails
    {
        public PaymentDetails(Guid id, DateTime timeStamp)
        {
            Id = id;
            TimeStamp = timeStamp;
        }

        public DateTime TimeStamp { get; set; }
        public Guid Id { get; }
        public string CardNumber { get; set; }
        public int Amount { get; set; }
        public string CVV { get; set; }
        public PaymentStatus Status { get; set; }
        public string BankResponseId { get; set; }
    }
}
