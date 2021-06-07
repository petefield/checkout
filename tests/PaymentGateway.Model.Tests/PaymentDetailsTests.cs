using System;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using PaymentGateway.Models.Validation;

namespace PaymentGateway.Models.Tests
{
    public class PaymentDetailsTests
    {
        IServiceProvider _serviceProvider;

        public PaymentDetailsTests()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            var validCurrencyCodeProvider = Substitute.For<IValidCurrencyCodeProvider>();
            validCurrencyCodeProvider.ValidCurrencyCodes.Returns( new[] { "GBP","USD"});
            _serviceProvider.GetService(typeof(IValidCurrencyCodeProvider)).Returns(validCurrencyCodeProvider);
        }   

        [Fact]
        public void Constructor_WhenCalled_ShouldInitialiseProperties()
        {
            var id = Guid.NewGuid();
            var time = DateTime.Now;
            var request = new PaymentDetails(id, time);
            Assert.Equal(id, request.Id);
            Assert.Equal(time, request.TimeStamp);
        }
    }
}
