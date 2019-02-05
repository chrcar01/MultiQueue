using MultiQueue.Core.Messages;
using System;
using System.Threading.Tasks;

namespace MultiQueue.Core.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        public Task<Payment> SavePayment(Payment payment)
        {
            payment.Id = Guid.NewGuid();
            return Task.FromResult(payment);
        }
    }
}