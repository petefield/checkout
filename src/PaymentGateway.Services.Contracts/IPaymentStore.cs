using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Contracts;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Data.Contracts
{
    public interface IPaymentStore
    {
        Task<IPaymentDetails> AddPaymentRequest(IPaymentRequest request);
        Task<IPaymentDetails> AddPaymentResponse(Guid requestId, IPaymentResponse response);
        Task<IPaymentDetails> Read(Guid requestId);
        Task<IEnumerable<IPaymentDetails>> Read();
    }
}
