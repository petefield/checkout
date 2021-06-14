using AcquiringBank.Contracts;
using PaymentGateway.Models.Contracts;
using PaymentGateway.Utils;
using System;

namespace PaymentGateway.Models
{
    public class PaymentDetails : IPaymentDetails
    {
        public PaymentDetails(IPaymentRequest request, IPaymentResponse response)
        {
            request.ArgumentNullCheck(nameof(request));
            response.ArgumentNullCheck(nameof(request));

            Id = request.RequestId;
            CardNumber = request.CardNumber.Length >= 4 ? request.CardNumber[^4..] : request.CardNumber;
            Amount = request.Amount;
            CVV = request.CVV;
            Outcome = response.Outcome;
            Reason = response.Reason;
            BankReference = response.Id;
            CurrencyCode = request.CurrencyCode;
        }

        public Guid Id { get; }
        public string CardNumber { get; }
        public int Amount { get; }
        public string CVV { get; }
        public Outcome Outcome { get; }
        public string Reason { get; }
        public string BankReference { get; }
        public DateTime Received { get; }
        public DateTime Processed { get; }
        public string CurrencyCode { get ; }
    }
}
