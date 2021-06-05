using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models.Contracts;
using PaymentGateway.Services.Contracts;
using AcquiringBank.Contracts;
using PaymentGateway.Models;

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
        public async Task<IPaymentDetail> Get(Guid PaymentRequestId)
        {
            var paymentDetail = await _paymentRepo.Read(PaymentRequestId);
            return paymentDetail;
        }

        [HttpPost]
        public async Task<IPaymentResponse> Post(IPaymentRequest request)
        {
            var requestId = Guid.NewGuid();
            var bankResponse = await SendRequestToBank(request);
            var response = Map(request, bankResponse);
            await _paymentRepo.AddPaymentDetails(request, response);
            return response;
        }

        private IPaymentResponse Map(IPaymentRequest request, IAcquiringBankResponse bankresponse) => new PaymentResponse()
        {
            Id = Guid.NewGuid(),
            RequestDetails = request,
            Reason = bankresponse.Reason,
            Status = (PaymentResponseStatus)bankresponse.Status
        };

        private async Task<IAcquiringBankResponse> SendRequestToBank(IPaymentRequest request)
        {
            return await _aquiringBank.CreatePayment(request.CardNumber, request.CVV, request.ExpiryDate.Year, request.ExpiryDate.Month, request.Amount, request.CurrencyCode );
        }
    }
}
