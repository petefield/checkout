using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using PaymentGateway.Data.Contracts;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Data
{
    public class PaymentStore : IPaymentStore
    {
        ConcurrentDictionary<Guid, IPaymentDetails> _store;

        public PaymentStore()
        {
            _store = new ConcurrentDictionary<Guid, IPaymentDetails>();
        }

        public async Task<IPaymentDetails> AddPaymentDetails(IPaymentDetails details)
        {
            _store.TryAdd(details.Id, details);
            return await Task.FromResult(details);
        }

        public async Task<IPaymentDetails> Read(Guid PaymentId)
        {
            _store.TryGetValue(PaymentId, out var p);
            return await Task.FromResult(p);
        }
    }
}
