using MultiQueue.Core.Messages;
using System.Threading.Tasks;

namespace MultiQueue.Core.Repositories
{
    public interface IPaymentsRepository
    {
        Task<Payment> SavePayment(Payment payment);
    }
}
