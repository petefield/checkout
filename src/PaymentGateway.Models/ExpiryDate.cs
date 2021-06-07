using PaymentGateway.Models.Contracts;

namespace PaymentGateway.Models
{
    public class ExpiryDate : IExpiryDate
    {
        public ExpiryDate() {}

        public ExpiryDate(int year, int month)
        {
            Year = year;
            Month = month;
        }

        public int Year { get; set; }
        public int Month { get; set; }

        public override string ToString() => $"{Month}/{Year}";
    }
}
