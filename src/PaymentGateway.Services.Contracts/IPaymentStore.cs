using System;
using System.Threading.Tasks;
using AcquiringBank.Contracts;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Data.Contracts
{
    public interface IPaymentStore
    {
        Task<IPaymentRequest> AddPaymentRequest(IPaymentRequest request);
        Task<IPaymentResponse> AddPaymentResponse(Guid requestId, IPaymentResponse response);
        Task<(IPaymentRequest paymentRequest, IPaymentResponse paymentResponse)?> Read(Guid requestId);
    }
}
