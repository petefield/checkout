using AcquiringBank.Contracts;

namespace AcquiringBank.InMemory
{
    public class Response : IAcquiringBankResponse
    {
        public string Id { get; set; }
        public IAcquiringBankRequestStatus Status { get; set; }
        public string Reason { get; set; }
    }
}
