using System.Threading.Tasks; 

namespace AcquiringBank.Contracts
{
    public interface IAcquiringBank 
    {
        Task<IAcquiringBankResponse> CreatePayment(string creditCardNumber, string CVV, int expiryYear, int expiryMonth, decimal amount, string currency);
    }
}
