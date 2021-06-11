using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AcquiringBank.Contracts;
using PaymentGateway.Models;
using PaymentGateway.Data.Contracts;
using PaymentGateway.Utils;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IAcquiringBank _aquiringBank;
        private readonly IPaymentStore _paymentRepo;

        public PaymentsController(ILogger<PaymentsController> logger, IAcquiringBank aquiringBank, IPaymentStore paymentRepo)
        {
            _logger = logger.ArgumentNullCheck(nameof(logger));
            _aquiringBank = aquiringBank.ArgumentNullCheck(nameof(aquiringBank));
            _paymentRepo = paymentRepo.ArgumentNullCheck(nameof(paymentRepo));
        }

        [HttpGet("{paymentId}")]
        public async Task<ActionResult<PaymentDetails>> Get(Guid paymentId)
        {
            _logger.LogInformation($"Payment details for {paymentId} requester.");
            var payment = await _paymentRepo.Read(paymentId);
            if (payment == null) return NotFound();
            return new PaymentDetails(payment.Value.paymentRequest, payment.Value.paymentResponse);
        }

        /// <summary>
        /// Get payment details
        /// Returns the details of the payment with the specified identifier string.
        /// </summary>
        /// /// <remarks>
        /// More elaborate description
        /// </remarks>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult<PaymentDetails>> Post(PaymentRequest paymentRequest)
        {
            _logger.LogInformation($"Valid payment request recieved.");
            var request = await _paymentRepo.AddPaymentRequest(paymentRequest);
            var bankResponse = await SendRequestToBank(request);
            await _paymentRepo.AddPaymentResponse(request.RequestId, bankResponse);
            return new PaymentDetails(request, bankResponse);
        }

        private async Task<IPaymentResponse> SendRequestToBank(IPaymentRequest request)
        {
            _logger.LogInformation($"Payment request {request.RequestId} sent to bank");
            return await _aquiringBank.CreatePayment(request.CardNumber, request.CVV, request.ExpiryDate.Year, request.ExpiryDate.Month, request.Amount, request.CurrencyCode );
        }
    }
}
