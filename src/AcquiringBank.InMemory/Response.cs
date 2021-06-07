using AcquiringBank.Contracts;
using System;

namespace AcquiringBank.InMemory
{
    public class Response : IPaymentResponse
    {
        public Response() : this(Outcome.Success, null)
        {
        }

        public Response(Outcome outcome, string reason)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Outcome = outcome;
            this.Reason = reason;
            this.TimeStamp = DateTime.UtcNow;
        }

        public string Id { get; }
        public Outcome Outcome { get; }
        public string Reason { get; }
        public DateTime TimeStamp { get; }
    }
}
