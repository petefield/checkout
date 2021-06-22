using AcquiringBank.Contracts;
using Amazon.DynamoDBv2.DataModel;
using PaymentGateway.Models.Contracts;
using System;

namespace PaymentGateway.Data.InMemory
{
    internal class PaymentRecord : IPaymentDetails
    {
        public PaymentRecord()
        {
            
        }

        public PaymentRecord(IPaymentRequest request)
        {
            Id = Guid.NewGuid();
            CardNumber = request.CardNumber; 
            Amount = request.Amount; 
            CurrencyCode = request.CurrencyCode;
            CVV = request.CVV;
            Received  = DateTime.UtcNow;
        }

        public void AddPaymentResponse(IPaymentResponse response){
            Outcome = response.Outcome;
            Reason = response.Reason;
        }

        [DynamoDBHashKey] //Partition key
        public Guid Id { get; internal set; }

        [DynamoDBProperty]
        public string CardNumber { get;  internal set; }

        [DynamoDBProperty]
        public int Amount { get; internal set;  }

        [DynamoDBProperty]
        public string CVV { get;  internal set; }

        [DynamoDBProperty]
        public Outcome Outcome { get; internal set;}

        [DynamoDBProperty]
        public string Reason { get; internal set; }

        [DynamoDBProperty]
        public string BankReference { get; internal set;}
        
        [DynamoDBProperty]
        public DateTime Received { get; internal set; }
        
        [DynamoDBProperty]
        public DateTime Processed { get; internal set; }
        
        [DynamoDBProperty]
        public string CurrencyCode { get ; internal set; }
    }
}
