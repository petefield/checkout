using System;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using PaymentGateway.Models.Validation;

namespace PaymentGateway.Models.Tests
{
    public class PaymentRequestValidationTests
    {
        IServiceProvider _serviceProvider;

        public PaymentRequestValidationTests()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            var validCurrencyCodeProvider = Substitute.For<IValidCurrencyCodeProvider>();
            validCurrencyCodeProvider.ValidCurrencyCodes.Returns( new[] { "GBP","USD"});
            _serviceProvider.GetService(typeof(IValidCurrencyCodeProvider)).Returns(validCurrencyCodeProvider);
        }   

        [Fact]
        public void ValidPaymentRequest_ShouldPassValidation()
        {
            var expiryDate = DateTime.Now.AddYears(1);
            var request = new PaymentRequest(){
                CardNumber = "12345674",
                CVV = "123",
                CurrencyCode = "GBP",
                Amount = 100,
                ExpiryDate = new ExpiryDate { Year = expiryDate.Year, Month = expiryDate.Month }
            };

            var errors = ValidateModel(request);
            Assert.False(errors.Any());
        }

        [Theory]
        [InlineData("12345674", "123",      0,          2022, 12, "GBP",  new[] { nameof(PaymentRequest.Amount) })]
        [InlineData("12345674", "123",      1000000000, 2022, 12, "GBP",  new[] { nameof(PaymentRequest.Amount) })]
        [InlineData("",         "123",      100,        2022, 12, "GBP",  new[] { nameof(PaymentRequest.CardNumber) })]
        [InlineData(null,       "123",      100,        2022, 12, "GBP",  new[] { nameof(PaymentRequest.CardNumber) })]
        [InlineData("12345674", "",         100,        2022, 12, "GBP",  new[] { nameof(PaymentRequest.CVV) })]
        [InlineData("12345674", null,       100,        2022, 12, "GBP",  new[] { nameof(PaymentRequest.CVV) })]
        [InlineData("12345674", "01234",    100,        2022, 12, "GBP",  new[] { nameof(PaymentRequest.CVV) })]
        [InlineData("12345674", "123",      100,        2022, 12, "Fish", new[] { nameof(PaymentRequest.CurrencyCode) })]
        [InlineData("12345674", "123",      100,        2022, 12, "",     new[] { nameof(PaymentRequest.CurrencyCode) })]
        [InlineData("12345674", "123",      100,        2022, 12, null,   new[] { nameof(PaymentRequest.CurrencyCode)  })]
        [InlineData("12345674", "123",      100,        2021, 01, "GBP",  new[] { nameof(PaymentRequest.ExpiryDate) })]
        public void InvalidPaymentRequest_FailsValidation(string cardNumber, string cvv, int amount, int year, int month, string currency, string[] invalidMembers)
        {
            var request = new PaymentGateway.Models.PaymentRequest()
            {
                CardNumber = cardNumber,
                CVV = cvv,
                CurrencyCode = currency,
                Amount = amount,
                ExpiryDate = new ExpiryDate { Year = year, Month = month }
            };

            var errors = ValidateModel(request);

            var error = errors.Single();
            Assert.Equal(invalidMembers, error.MemberNames);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, _serviceProvider, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
