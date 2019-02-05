using System.Threading.Tasks;
using MultiQueue.Core.Messages;

namespace MultiQueue.Core.Repositories
{
    public interface IOrdersRepository
    {
        Task<Order> Save(Order order);
    }
}