using AcquiringBank.Contracts;
using PaymentGateway.Models.Contracts;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PaymentGateway
{
    public class Client
    {
        private readonly Uri _url;
        private readonly string _key;

        public Client(string url, string key) : this(new Uri(url), key) { }

        public Client(Uri url, string key)
        {
            _url = url;
            _key = key;
        }

        private class PaymentDetails : IPaymentDetails
        {
            public Guid Id  {get; set; }

            public string CardNumber { get; set; }

            public int Amount { get; set; }

            public string CVV { get; set; }

            public Outcome Outcome { get; set; }

            public string Reason { get; set; }

            public string BankReference { get; set; }

            public DateTime Received { get; set; }

            public DateTime Processed { get; set; }
            public string CurrencyCode { get; set; }
        }

        public async Task<IPaymentDetails> RequestPayment(IPaymentRequest requestDetails) {

            AsyncRetryPolicy retry = Policy
              .Handle<HttpRequestException>()
              .RetryAsync(3);


            var httpClient = HttpClientFactory.Create();
            httpClient.BaseAddress = _url;

            var paymentDetails = await retry.ExecuteAsync(async () => {
                var response = await httpClient.PostAsJsonAsync("/payments", requestDetails);
                try{
                    return await response.Content.ReadAsAsync<PaymentDetails>();
                }
                catch(Exception){
                    var responsebody = await response.Content.ReadAsStringAsync();
                    throw new Exception(responsebody);
                }
            });

            return paymentDetails;

        }

        public async Task<IPaymentDetails> GetPaymentDetails(Guid paymentId)
        {

            AsyncRetryPolicy retry = Policy
              .Handle<HttpRequestException>()
              .RetryAsync(3);

           var httpClient = HttpClientFactory.Create();
            httpClient.BaseAddress = _url;

            var paymentDetails = await retry.ExecuteAndCaptureAsync(async () => {
                return await httpClient.GetFromJsonAsync<PaymentDetails> ($"/payments/{paymentId}");
            });

            return paymentDetails.Result;

        }
    }
}
