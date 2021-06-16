using AcquiringBank.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace AcquiringBank.InMemory
{
    public class InMemoryAcquiringBank : IAcquiringBank
    {
        public async Task<IPaymentResponse> CreatePayment(string creditCardNumber, string CVV, int expiryYear, int expiryMonth, decimal Amount, string currency)
        {
            Response response;

            switch (CVV.Last())
            {
                case '0':
                    response = new Response(Outcome.Failed, "Insufficient Funds.");
                    break;
                case '1':
                    response = new Response(Outcome.Failed, "Fraudulent.");
                    break;
                default:
                    response = new Response();
                    break;
            }
            await Task.Delay(2000);
            return response;
        }
    }
}
