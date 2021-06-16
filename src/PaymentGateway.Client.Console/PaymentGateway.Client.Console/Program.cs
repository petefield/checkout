using System;
using System.Threading.Tasks;

namespace PaymentGateway.Clients.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new PaymentGateway.Client("https://checkout-paymentgateway.azurewebsites.net/","");
            var response = await client.RequestPayment(new PaymentGateway.Models.PaymentRequest() { 
                Amount = 101,
                CardNumber = "12345674",
                CVV = "123", 
                CurrencyCode = "GBP",
                ExpiryDate = new Models.ExpiryDate(year: 2022, month: 12),
            });

            System.Console.WriteLine($"{response.Received } : Payment Request ID  {response.Id} for {response.Amount / 100M} {response.CurrencyCode} against card {response.CardNumber}.");
            System.Console.WriteLine($"{response.Processed } : The request outcome was : {response.Outcome} .");
            if (response.Outcome != AcquiringBank.Contracts.Outcome.Success)
            {
                System.Console.WriteLine($"The reason given was {response.Reason}.");
            }
            
            System.Console.ReadLine();
            var x = await client.GetPaymentDetails(response.Id);
        }
    }
}
