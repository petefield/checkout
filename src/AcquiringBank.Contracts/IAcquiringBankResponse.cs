using System;

namespace AcquiringBank.Contracts
{
    public interface IAcquiringBankResponse
    {
        string Id { get; }
        IAcquiringBankRequestStatus Status { get; }
        string Reason { get; }
    }
}
