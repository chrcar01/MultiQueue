using System;
using System.Threading.Tasks;
using MultiQueue.Core.Messages;
using MultiQueue.Core.Models.Shared;
using MultiQueue.Core.Queues;
using MultiQueue.Core.Repositories;

namespace MultiQueue.Core.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _repo;
        private readonly OrderQueue _queue;

        public OrdersService(IOrdersRepository repo, OrderQueue queue)
        {
            _repo = repo;
            _queue = queue;
        }

        public Task SayHello(Order order)
        {
            Console.WriteLine($"Thanks for your order {order.Id}");
            return Task.CompletedTask;
        }
        public async Task<ServiceResponse<Order>> Submit(Order order)
        {
            var response = new ServiceResponse<Order>();
            try
            {
                var saveOrderTask = _repo.Save(order);
                var submitOrderTask = _queue.Send(order);
                await Task.WhenAll(saveOrderTask, submitOrderTask);
                response.Data = saveOrderTask.Result;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
    }
}
