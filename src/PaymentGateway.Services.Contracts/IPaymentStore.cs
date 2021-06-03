using System;
using System.Threading.Tasks;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Services.Contracts
{
    public interface IPaymentStore
    {
        Task<IPaymentRequest> AddRequest(IPaymentRequest request);
        Task<IPaymentRequest> AddResponse(IPaymentResponse request);
        Task<IPaymentDetail> Read(Guid PaymentId);
    }
}
