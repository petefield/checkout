using System;
using Xunit;
using NSubstitute;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Services.Contracts;
using AcquiringBank.Contracts;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models.Contracts;


namespace tests
{
    public class PaymentControllerTests
    {
        IPaymentStore _store;
        IPaymentRequestValidator _validator; 
        IPaymentProcessor _processor;
        PaymentGateway.Api.Controllers.PaymentController _subject;
        ILogger<PaymentController> _logger; 
        IPaymentRequest _request;
        public PaymentControllerTests()
        {   
            _logger = Substitute.For<ILogger<PaymentController>>();
            _store = Substitute.For<IPaymentStore>();
            _validator = Substitute.For<IPaymentRequestValidator>();
            var validationResult = Substitute.For<IValidationResult>();
            validationResult.IsValid.Returns(true);
            _validator.Validate(Arg.Any<IPaymentRequest>()).Returns(validationResult);

            _processor =  Substitute.For<IPaymentProcessor>();
            _request = Substitute.For<IPaymentRequest>();

            _subject = new PaymentController(_logger,_validator, _processor, _store );
        }

        [Fact]
        public void PaymentController_WhenRequestIsPosted_ShouldValidateRequest()
        {
            _subject.Post(_request);
            _validator.Received().Validate(Arg.Is(_request));
        }

        [Fact]
        public void PaymentController_WhenValidRequestIsPosted_ShouldStoreRequest()
        {
            _subject.Post(_request);
            _store.Received().AddRequest(Arg.Is(_request));
        }

        [Fact]
        public void PaymentController_WhenRequestIsPosted_ShouldPassRequestToAcquiringBank()
        {
            _subject.Post(_request);
            _processor.Received().CreatePayment(Arg.Is<IPaymentProcessingRequest>(r => r.amount == _request.Amount));     
        }
    }
}
