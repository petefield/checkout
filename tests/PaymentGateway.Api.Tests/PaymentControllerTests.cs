using System;
using Xunit;
using NSubstitute;
using PaymentGateway.Api.Controllers;
using AcquiringBank.Contracts;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models.Contracts;
using System.Threading.Tasks;
using PaymentGateway.Models;
using PaymentGateway.Data.Contracts;

namespace PaymentGateway.Api.Tests
{
    public class PaymentControllerTests
    {
        IPaymentStore _store;
        IAcquiringBank _acquiringBank;
        PaymentController _subject;
        ILogger<PaymentController> _logger;
        public PaymentControllerTests()
        {
            _logger = Substitute.For<ILogger<PaymentController>>();
            _store = Substitute.For<IPaymentStore>();
            _acquiringBank = Substitute.For<IAcquiringBank>();
            _subject = new PaymentController(_logger, _acquiringBank, _store);
        }

        [Fact]
        public async Task PaymentController_WhenValidResponseIsRecieved_ShouldStoreDetails()
        {
            var request = CreatePaymentRequest();
            await _subject.Post(request);
            await _store.Received().AddPaymentDetails(Arg.Any<IPaymentDetails>());
        }

        [Fact]
        public async Task PaymentController_WhenValidRequestIsPosted_ShouldPassRequestToAcquiringBank()
        {
            var request = CreatePaymentRequest();

            await _subject.Post(request);

            await _acquiringBank.Received().CreatePayment(
                Arg.Is<string>(r => r == request.CardNumber),
                Arg.Is<string>(r => r == request.CVV),
                Arg.Is<int>(r => r == request.ExpiryDate.Year),
                Arg.Is<int>(r => r == request.ExpiryDate.Month),
                Arg.Is<decimal>(r => r == request.Amount),
                Arg.Is<string>(r => r == request.CurrencyCode));
        }

          [Fact]
        public async Task PaymentController_WhenGetCalled_ShouldObtainPaymentDetailsFromStore()
        {
            var paymentId = Guid.NewGuid();
            await _subject.Get(paymentId);
            await _store.Received().Read(paymentId);
        }

        private PaymentRequest CreatePaymentRequest(bool valid = true) =>   new PaymentRequest {
            CardNumber = valid ? "12345674" : null,
            CVV = "123",
            CurrencyCode = "GBP",
            Amount = 100,
            ExpiryDate = new ExpiryDate { Year = DateTime.Now.AddYears(1).Year, Month = 1 }
        };
    }
}
