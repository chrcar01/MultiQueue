using System.Threading.Tasks;
using MultiQueue.Core.Messages;
using MultiQueue.Core.Models.Shared;

namespace MultiQueue.Core.Services
{
    public interface IPaymentsService
    {
        Task<ServiceResponse<Payment>> Submit(Payment payment);
        Task SayHello(Payment payment);
    }
}