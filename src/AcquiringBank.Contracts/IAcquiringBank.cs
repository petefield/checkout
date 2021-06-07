using System.Threading.Tasks; 

namespace AcquiringBank.Contracts
{
    public interface IAcquiringBank 
    {
        Task<IPaymentResponse> CreatePayment(string creditCardNumber, string CVV, int expiryYear, int expiryMonth, decimal amount, string currency);
    }
}
