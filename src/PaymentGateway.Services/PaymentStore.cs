using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using AcquiringBank.Contracts;
using PaymentGateway.Data.Contracts;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Data.InMemory
{
    public class PaymentStore : IPaymentStore
    {
        ConcurrentDictionary<Guid, IPaymentRequest> _requests;
        ConcurrentDictionary<Guid, IPaymentResponse> _responses;

        public PaymentStore()
        {
            _requests = new ConcurrentDictionary<Guid, IPaymentRequest>();
            _responses = new ConcurrentDictionary<Guid, IPaymentResponse>();
        }

        public async Task<IPaymentRequest> AddPaymentRequest(IPaymentRequest request)
        {
            request.RequestId = Guid.NewGuid();
            _requests.TryAdd(request.RequestId, request);
            return await Task.FromResult(request);
        }

        public async Task<IPaymentResponse> AddPaymentResponse(Guid requestId, IPaymentResponse response)
        {
            _responses.TryAdd(requestId, response);
            return await Task.FromResult(response);
        }

        public async Task<(IPaymentRequest paymentRequest, IPaymentResponse paymentResponse)?> Read(Guid paymentId)
        {
            _requests.TryGetValue(paymentId, out var request);
            _responses.TryGetValue(paymentId, out var response);
            return await Task.FromResult((request, response));
        }
    }
}
