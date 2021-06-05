using PaymentGateway.Models.Contracts;
using PaymentGateway.Services.Contracts;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public class PaymentStore : IPaymentStore
    {

        ConcurrentDictionary<Guid, IPaymentDetail> _store;

        public PaymentStore()
        {
            _store = new ConcurrentDictionary<Guid, IPaymentDetail>();
        }

        public async Task<IPaymentDetail> AddPaymentDetails(IPaymentRequest request, IPaymentResponse response)
        {
            var details = new PaymentDetails(response.Id);
            details.CardNumber = request.CardNumber[^4..];
            details.CVV = request.CVV;
            details.Amount = request.Amount;
            details.Status = response.Status;
            _store.TryAdd(details.Id, details);
            return await Task.FromResult(details);
        }

        public async Task<IPaymentDetail> Read(Guid PaymentId)
        {
            _store.TryGetValue(PaymentId, out var p);
            return await Task.FromResult( p);
        }
    }
}
