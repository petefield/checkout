using System;
using System.Threading.Tasks;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Data.Contracts
{
    public interface IPaymentStore
    {
        Task<IPaymentDetails> AddPaymentDetails(IPaymentDetails details);
        Task<IPaymentDetails> Read(Guid requestId);
    }
}
