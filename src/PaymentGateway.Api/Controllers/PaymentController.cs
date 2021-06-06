using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models.Contracts;
using AcquiringBank.Contracts;
using PaymentGateway.Models;
using PaymentGateway.Data.Contracts;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IAcquiringBank _aquiringBank;
        private readonly IPaymentStore _paymentRepo;

        public PaymentController(ILogger<PaymentController> logger,
            IAcquiringBank aquiringBank, 
            IPaymentStore paymentRepo)
        {
            _logger = logger;
            _aquiringBank = aquiringBank;
            _paymentRepo = paymentRepo;
        }

        [HttpGet]
        public async Task<IPaymentDetails> Get(Guid PaymentRequestId)
        {
            var paymentDetail = await _paymentRepo.Read(PaymentRequestId);
            return paymentDetail as PaymentDetails;
        }

        [HttpPost]
        public async Task<IPaymentDetails> Post(PaymentRequest request)
        {
            _logger.LogInformation("Payment Request Recieved");

            var bankResponse = await SendRequestToBank(request);

            var paymentDetail = await _paymentRepo.AddPaymentDetails(new PaymentDetails(Guid.NewGuid(), DateTime.UtcNow)
            {
                CardNumber = request.CardNumber,
                CVV = request.CVV,
                Amount = request.Amount,
                BankResponseId = bankResponse.Id
            });
            return paymentDetail;
        }

        private async Task<IAcquiringBankResponse> SendRequestToBank(PaymentRequest request)
        {
            return await _aquiringBank.CreatePayment(request.CardNumber, request.CVV, request.ExpiryDate.Year, request.ExpiryDate.Month, request.Amount, request.CurrencyCode );
        }
    }
}
