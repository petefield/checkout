using System;
using System.Collections.Generic;
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

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PaymentDetails>>> Get()
        {
            _logger.LogInformation($"Fetch all payment details.");
            var results = await _paymentRepo.Read();
            return Ok(results);
        }

        [HttpGet("{paymentId}")]
        public async Task<ActionResult<IPaymentDetails>> Get(Guid paymentId)
        {
            _logger.LogInformation($"Payment details for {paymentId} requester.");
            var paymentRecord = await _paymentRepo.Read(paymentId);
            if (paymentRecord == null) return NotFound();
            return Ok(paymentRecord);
        }

        [HttpPost]
        public async Task<ActionResult<IPaymentDetails>> Post(PaymentRequest paymentRequest)
        {
            _logger.LogInformation($"Valid payment request recieved.");
            var paymentRecord = await _paymentRepo.AddPaymentRequest(paymentRequest);
            _logger.LogInformation($"Request {paymentRecord.Id} recieved.");

            var bankResponse = await SendRequestToBank(paymentRequest);
            paymentRecord = await _paymentRepo.AddPaymentResponse(paymentRecord.Id, bankResponse);
            return CreatedAtAction(nameof(Get), new { paymentId = paymentRecord.Id }, paymentRecord);
        }

        private async Task<IPaymentResponse> SendRequestToBank(IPaymentRequest request)
        {
            _logger.LogInformation($"Payment request {request.RequestId} sent to bank");
            return await _aquiringBank.CreatePayment(request.CardNumber, request.CVV, request.ExpiryDate.Year, request.ExpiryDate.Month, request.Amount, request.CurrencyCode );
        }
    }
}
