using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcquiringBank.Contracts;
using PaymentGateway.Data.Contracts;
using PaymentGateway.Models.Contracts;
using Amazon.DynamoDBv2.DataModel;

namespace PaymentGateway.Data.InMemory
{
    public class PaymentStore : IPaymentStore
    {
        IDynamoDBContext _context;

        public PaymentStore(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<IPaymentDetails> AddPaymentRequest(IPaymentRequest request)
        {
            var record = new PaymentRecord(request);
            await _context.SaveAsync(record);
            return record;
        }

        public async Task<IPaymentDetails> AddPaymentResponse(Guid requestId, IPaymentResponse response)
        {
            var record = await ReadInternal(requestId);
            record.AddPaymentResponse(response);
            await _context.SaveAsync(record);
            return record;
        }

        private async Task<PaymentRecord> ReadInternal(Guid paymentId) => await _context.LoadAsync<PaymentRecord>(paymentId);

        public async Task<IPaymentDetails> Read(Guid paymentId) => await ReadInternal(paymentId);

        public async Task<IEnumerable<IPaymentDetails>> Read()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<PaymentRecord>(conditions).GetRemainingAsync();          
        }
    }
}
