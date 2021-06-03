using System;

namespace AcquiringBank.Contracts
{
    public interface IPaymentProcessingRequest
    {
        Guid PaymentRequestId { get; set; }
        Guid MerchantId { get; set; }
        decimal amount {get; set;}
    }
}
