using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models.Contracts;
using PaymentGateway.Services.Contracts;
using AcquiringBank.Contracts;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentRequestValidator _paymentRequestValidator;
        private readonly IPaymentProcessor _aquiringBank;
        private readonly IPaymentStore _paymentRepo;

        public PaymentController(ILogger<PaymentController> logger, 
            IPaymentRequestValidator paymentRequestValidator, 
            IPaymentProcessor aquiringBank, 
            IPaymentStore paymentRepo)
        {
            _logger = logger;
            _paymentRequestValidator = paymentRequestValidator;
            _aquiringBank = aquiringBank;
            _paymentRepo = paymentRepo;
        }

        [HttpGet]
        public async Task<IPaymentDetail> Get(Guid PaymentRequestId)
        {
            var paymentDetail = await _paymentRepo.Read(PaymentRequestId);
            return null;
        }

        [HttpPost]
        public async Task<IPaymentResponse> Post(IPaymentRequest request)
        {            
            await _paymentRequestValidator.Validate(request);
            await _paymentRepo.AddRequest(request);
            var bankResponse = await SendRequestToBank(request);
            var response = Map(bankResponse);
            await _paymentRepo.AddResponse(response);
            return response;
        }

        private IPaymentResponse Map(IPaymentProcessingResponse response) => null;

        private async Task<IPaymentProcessingResponse> SendRequestToBank(IPaymentRequest request)
        {
            return await _aquiringBank.CreatePayment(request.CardNumber, request.CVV, request.ExpiryDate.Year, request.ExpiryDate.Month, request.Amount, request.CurrencyCode );
        }
    }
}
