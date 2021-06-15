using System;
using Xunit;
using PaymentGateway.Validation;

namespace PaymentGateway.Models.Tests
{
    public class ExpiryDateAttributeTests
    {
        [Fact]
        public void ExpiryDateAttribute_IsValidWithFutureDate_ShouldReturnTrue()
        {
            var sut = new ExpiryDateValidatorAttribute();
            var result = sut.IsValid(new ExpiryDate(year: DateTime.UtcNow.AddYears(1).Year, month: 1));
            Assert.True(result);
        }

        [Fact]
        public void ExpiryDateAttribute_IsValidWithPastDate_ShouldReturnTrue()
        {
            var sut = new ExpiryDateValidatorAttribute();
            var result = sut.IsValid(new ExpiryDate(year: DateTime.UtcNow.AddYears(-1).Year, month: 1));
            Assert.False(result);
        }

        [Fact]
        public void ExpiryDateAttribute_IsValidWithInvalidDate_ShouldReturnFalse()
        {
            var sut = new ExpiryDateValidatorAttribute();
            var result = sut.IsValid(new ExpiryDate(year: 2020, month: 13));
            Assert.False(result);
        }

        [Fact]
        public void ExpiryDateAttribute_IsValidWithNullExpiryDate_ShouldReturnFalse()
        {
            var sut = new ExpiryDateValidatorAttribute();
            var result = sut.IsValid(null);
            Assert.False(result);
        }
    }
}
