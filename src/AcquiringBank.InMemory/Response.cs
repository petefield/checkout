using AcquiringBank.Contracts;
using System;

namespace AcquiringBank.InMemory
{
    public class Response : IAcquiringBankResponse
    {
        public Response()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Status = IAcquiringBankRequestStatus.Success;
        }

        public Response(IAcquiringBankRequestStatus status, string reason)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Status = status;
            this.Reason = reason;
        }

        public string Id { get; }
        public IAcquiringBankRequestStatus Status { get; }
        public string Reason { get; }
    }
}
