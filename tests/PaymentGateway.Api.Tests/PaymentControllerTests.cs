using System;
using Xunit;
using NSubstitute;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Services.Contracts;
using AcquiringBank.Contracts;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models.Contracts;
using System.Threading.Tasks;

namespace tests
{
    public class PaymentControllerTests
    {
        IPaymentStore _store;
        IAcquiringBank _acquiringBank;
        PaymentController _subject;
        ILogger<PaymentController> _logger;
        IPaymentRequest _request;
        public PaymentControllerTests()
        {
            _logger = Substitute.For<ILogger<PaymentController>>();
            _store = Substitute.For<IPaymentStore>();
            var validationResult = Substitute.For<IValidationResult>();
            validationResult.IsValid.Returns(true);
            _acquiringBank = Substitute.For<IAcquiringBank>();
            _request = Substitute.For<IPaymentRequest>();
            _subject = new PaymentController(_logger, _acquiringBank, _store);
        }

        [Fact]
        public async Task PaymentController_WhenResponseIsRecieved_ShouldStoreDetails()
        {
            await _subject.Post(_request);
            await _store.Received().AddPaymentDetails(Arg.Any<IPaymentRequest>(), Arg.Any<IPaymentResponse>());
        }

        [Fact]
        public async Task PaymentController_WhenRequestIsPosted_ShouldPassRequestToAcquiringBank()
        {
            await _subject.Post(_request);
            await _acquiringBank.Received().CreatePayment(
                Arg.Is<string>(r => r == _request.CardNumber),
                Arg.Is<string>(r => r == _request.CVV),
                Arg.Is<int>(r => r == _request.ExpiryDate.Year),
                Arg.Is<int>(r => r == _request.ExpiryDate.Month),
                Arg.Is<decimal>(r => r == _request.Amount),
                Arg.Is<string>(r => r == _request.CurrencyCode));
        }

          [Fact]
        public async Task PaymentController_WhenGetCalled_ShouldObtainPaymentDetailsFromStore()
        {
            var paymentId = Guid.NewGuid();
            await _subject.Get(paymentId);
            await _store.Received().Read(paymentId);
        }
    }
}
