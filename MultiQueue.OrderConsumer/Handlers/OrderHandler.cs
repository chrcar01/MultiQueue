using MultiQueue.Core.Messages;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using MultiQueue.Core.Services;

namespace MultiQueue.OrderConsumer.Handlers
{
    public class OrderHandler : IHandleMessages<Order>
    {
        private readonly IOrdersService _service;

        public OrderHandler(IOrdersService service)
        {
            _service = service;
        }
        public async Task Handle(Order message)
        {
            await _service.SayHello(message);
        }
    }
}
