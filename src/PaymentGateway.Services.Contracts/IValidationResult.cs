using System;
using System.Threading.Tasks;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Services.Contracts
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        string reason {get; }
    }
}
