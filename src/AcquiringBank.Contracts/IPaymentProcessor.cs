using System.Threading.Tasks; 

namespace AcquiringBank.Contracts
{
    public interface IPaymentProcessor 
    {
        Task<IPaymentProcessingResponse> CreatePayment(IPaymentProcessingRequest request);
    }
}
