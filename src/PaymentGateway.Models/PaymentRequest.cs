using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Models
{
    public class PaymentRequest : IPaymentRequest
    {
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string CVV {get; set;}
        
        [Required]
        public IExpiryDate ExpiryDate { get; set; }

        [Range(1, 999999999)]
        public int Amount {get; set; }

        [Required]
        public string CurrencyCode{get; set;}
    }
}
