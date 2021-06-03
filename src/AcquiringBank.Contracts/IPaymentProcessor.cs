using System.Threading.Tasks; 

namespace AcquiringBank.Contracts
{
    public interface IPaymentProcessor 
    {
        Task<IPaymentProcessingResponse> CreatePayment(string creditCardNumber, string CVV, int expiryYear, int expiryMonth, decimal Amount, string currency);
    }
}
