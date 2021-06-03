using System;
using System.Threading.Tasks;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Services.Contracts
{
    public interface IPaymentRequestValidator
    {
        Task<IValidationResult> Validate(IPaymentRequest request);
    }
}
