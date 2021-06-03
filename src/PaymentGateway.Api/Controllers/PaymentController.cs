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
        public IPaymentDetail Get(Guid PaymentRequestId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IPaymentResponse> Post(IPaymentRequest request)
        {            
            await _paymentRequestValidator.Validate(request);
            await _paymentRepo.AddRequest(request);
            await _aquiringBank.CreatePayment(request.CardNumber, request.CVV, request.ExpiryDate.Year,request.ExpiryDate.Month, request.Amount, request.CurencyCode );
            throw new NotImplementedException();
        }
    }
}
