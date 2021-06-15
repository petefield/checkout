using System;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using PaymentGateway.Validation;

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
    }
}
