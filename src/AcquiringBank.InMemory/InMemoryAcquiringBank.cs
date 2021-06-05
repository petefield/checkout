using AcquiringBank.Contracts;
using System;
using System.Threading.Tasks;

namespace AcquiringBank.InMemory
{
    public class InMemoryAcquiringBank : IAcquiringBank
    {
        public async Task<IAcquiringBankResponse> CreatePayment(string creditCardNumber, string CVV, int expiryYear, int expiryMonth, decimal Amount, string currency)
        {
            return await Task.FromResult(new Response { 
                Id = Guid.NewGuid().ToString(),
                Status = IAcquiringBankRequestStatus.Failed,
                Reason = "Test System"
            });
        }
    }
}
