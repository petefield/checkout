using System;
using Xunit;
using PaymentGateway.Validation;
using System.ComponentModel.DataAnnotations;
using NSubstitute;

namespace PaymentGateway.Models.Tests
{
    public class CurrencyCodeAttributeTests
    {
        private readonly IServiceProvider _serviceProvider;

        public CurrencyCodeAttributeTests()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            var ccp = Substitute.For<IValidCurrencyCodeProvider>();
            ccp.ValidCurrencyCodes.Returns(new[] { "GBP" });
            _serviceProvider.GetService(typeof(IValidCurrencyCodeProvider)).Returns(ccp);
        }

        [Fact]
        public void CurrencyCodeAttribute_GetValidationResult_WithValidCode_ShouldPass()
        {
            var sut = new CurrencyCodeAttribute();
            string value = "GBP";
            var result = sut.GetValidationResult(value, new ValidationContext(value, _serviceProvider, null));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void CurrencyCodeAttribute_GetValidationResult_WhenNoCurrencyCodeProviderSupplied_ShouldThrow()
        {
            var sut = new CurrencyCodeAttribute();
            string value = "GBP";
            var serviceProvider = Substitute.For<IServiceProvider>();
            Assert.Throws<InvalidOperationException>(() => sut.GetValidationResult(value, new ValidationContext(value, serviceProvider, null))) ;
        }

        [Fact]
        public void CurrencyCodeAttribute_GetValidationResult_WithInvalidCode_ShouldFail()
        {
            var sut = new CurrencyCodeAttribute();
            string value = "Invalid";
            var result = sut.GetValidationResult(value, new ValidationContext(value, _serviceProvider, null));
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void CurrencyCodeAttribute_GetValidationResult_WithNonStringInput_ShouldFail()
        {
            var sut = new CurrencyCodeAttribute();
            int value = 0;
            var result = sut.GetValidationResult(value, new ValidationContext(value, _serviceProvider, null));
            Assert.NotEqual(ValidationResult.Success, result);
        }
    }
}
