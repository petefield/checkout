using System;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using PaymentGateway.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Models.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ValidPaymentRequest_ShouldPassValidation()
        {
            var s = new PaymentGateway.Models.PaymentRequest(){
                CardNumber = "12345674",
                CVV = "123",
                CurrencyCode = "GBP",
                Amount = 100, 
                ExpiryDate = new ExpiryDate {  
                    Year = 2021, 
                    Month = 12,
                }
            };

            var errors = ValidateModel(s);
            Assert.False(errors.Any());
        }

        [Theory]
        [InlineData("12345674", "123", 0, 20, 12, "GBP", new[] { "Amount" })]
        [InlineData("12345674", "123", 1000000000, 20, 12, "GBP", new[] { "Amount" })]
        [InlineData("", "123", 100, 20, 12, "GBP", new[] { "CardNumber" })]
        [InlineData(null, "123", 100, 20, 12, "GBP", new[] { "CardNumber" })]
        [InlineData("12345674", "", 100, 20, 12, "GBP", new[] { "CVV" })]
        [InlineData("12345674", null, 100, 20, 12, "GBP", new[] { "CVV" })]
        [InlineData("12345674", "123", 100, 20, 12, "Fish", new[] { "CurrencyCode" })]
        [InlineData("12345674", "123", 100, 20, 12, "", new[] { "CurrencyCode" })]
        [InlineData("12345674", "123", 100, 20, 12, null, new[] { "CurrencyCode" })]
        public void InvalidPaymentRequest_FailsValidation(string cardNumber, string cvv, int amount, int year, int month, string currency, string[] invalidMembers)
        {
            var s = new PaymentGateway.Models.PaymentRequest()
            {
                CardNumber = cardNumber,
                CVV = cvv,
                CurrencyCode = "GBP",
                Amount = amount,
                ExpiryDate = new ExpiryDate
                {
                    Year = year,
                    Month = month,
                }
            };
            var error = ValidateModel(s).Single();
            Assert.Equal(error.MemberNames, invalidMembers);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
