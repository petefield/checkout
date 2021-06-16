using System;
using System.Threading.Tasks;
using System.Linq;

namespace PaymentGateway.Clients.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new PaymentGateway.Client("https://checkout-paymentgateway.azurewebsites.net/","");

            var tasks = System.Linq.Enumerable.Range(0,5000).Select(async i => {



                    var response = await client.RequestPayment(new PaymentGateway.Models.PaymentRequest() { 
                    Amount = 101,
                    CardNumber = "12345674",
                    CVV = "123", 
                    CurrencyCode = "GBP",
                    ExpiryDate = new Models.ExpiryDate(year: 2022, month: 12),
                });

                System.Console.WriteLine($"{response.Received } : Payment Request ID  {response.Id} for {response.Amount / 100M} {response.CurrencyCode} against card {response.CardNumber} => {response.Outcome} at {response.Processed}.");
            });
            
            await Task.WhenAll(tasks);
            System.Console.Write("done");

            System.Console.ReadLine();
        }
    }
}
