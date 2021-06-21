using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using PaymentGateway.Models;
using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Clients.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new PaymentGateway.Client("http://3.139.92.15/","");

            var payments = new List<IPaymentDetails>();
            var batchSize = 1;
    
            for(int i = 0; i < 1; i++)
            {
                var tasks = System.Linq.Enumerable.Range(0, batchSize).Select(async i =>await client.RequestPayment(new PaymentRequest { 
                    Amount = 101,
                    CardNumber = "12345674",
                    CVV = "123", 
                    CurrencyCode = "GBP",
                    ExpiryDate = new Models.ExpiryDate(year: 2022, month: 12)  }));
                
                var results = await Task.WhenAll(tasks);
                payments.AddRange(await Task.WhenAll(tasks));

                System.Console.WriteLine(payments.Count);
            }   
            
            System.Console.Write("done"); 
            System.Console.ReadLine();
        }
    }
}
