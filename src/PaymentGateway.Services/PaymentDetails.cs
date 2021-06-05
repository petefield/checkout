using PaymentGateway.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public class PaymentDetails : IPaymentDetail
    {
        public PaymentDetails(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set;  }
        public string CardNumber { get; set;  }
        public int Amount { get; set;  }
        public string CVV { get; set;  }
        public PaymentResponseStatus Status { get; set;  }
    }
}
