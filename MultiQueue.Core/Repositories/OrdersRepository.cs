using MultiQueue.Core.Messages;
using System;
using System.Threading.Tasks;

namespace MultiQueue.Core.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        public Task<Order> Save(Order order)
        {
            order.Id = Guid.NewGuid();
            return Task.FromResult(order);
        }
    }
}
