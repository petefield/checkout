using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PaymentGateway.Models.Contracts;
using PaymentGateway.Models.Validation;

namespace PaymentGateway.Models
{
    public class PaymentRequest : IPaymentRequest
    {
        public PaymentRequest()
        {
            this.Received = DateTime.UtcNow;
        }

        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; }

        [Required]
        [ExpiryDate]
        public ExpiryDate ExpiryDate { get; set; }

        [Range(1, 999999999)]
        public int Amount { get; set; }

        [Required]
        [CurrencyCode]
        public string CurrencyCode { get; set; }

        [JsonIgnore]
        public Guid RequestId {get; set; }

        [JsonIgnore]
        public DateTime Received { get; set; }

        public DateTime TimeStamp { get; set; }

        IExpiryDate IPaymentRequest.ExpiryDate { get => ExpiryDate; set => ExpiryDate = value as ExpiryDate; }
    }
}
