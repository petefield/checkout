using System;
using System.Threading.Tasks;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Services.Contracts
{
    public interface IPaymentStore
    {
        Task<IPaymentDetail> AddPaymentDetails(IPaymentRequest request, IPaymentResponse response);
        Task<IPaymentDetail> Read(Guid requestId);
    }
}
