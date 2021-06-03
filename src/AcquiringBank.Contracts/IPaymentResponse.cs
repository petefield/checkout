using System;

namespace AcquiringBank.Contracts
{
    public interface IPaymentProcessingResponse
    {
        Guid PaymentRequestId { get; }
        Guid MerchantId { get; }
        PaymentProcessingResponseStatus Status { get; }
        string Reason { get; }
    }
}
