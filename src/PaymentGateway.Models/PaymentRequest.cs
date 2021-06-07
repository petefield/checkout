﻿using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Models.Contracts;
using PaymentGateway.Models.Validation;

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
        [ExpiryDate]
        public ExpiryDate ExpiryDate { get; set; }

        [Range(1, 999999999)]
        public int Amount {get; set; }

        [Required]
        [CurrencyCode]
        public string CurrencyCode{get; set;}
        IExpiryDate IPaymentRequest.ExpiryDate { get => ExpiryDate; set => ExpiryDate = value as ExpiryDate; }
    }
}
