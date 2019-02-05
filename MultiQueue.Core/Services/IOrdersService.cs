using System.Threading.Tasks;
using MultiQueue.Core.Messages;
using MultiQueue.Core.Models.Shared;

namespace MultiQueue.Core.Services
{
    public interface IOrdersService
    {
        Task<ServiceResponse<Order>> Submit(Order order);
        Task SayHello(Order order);
    }
}