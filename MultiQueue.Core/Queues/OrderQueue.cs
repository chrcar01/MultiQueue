using Rebus.Bus;
using System;
using System.Threading.Tasks;

namespace MultiQueue.Core.Queues
{
    public class OrderQueue : IDisposable
    {
        private readonly IBus _bus;
        
        public OrderQueue(IBus bus)
        {
            _bus = bus;
        }

        public async Task Send(object message)
        {
            if (_bus != null)
            {
                await _bus.Advanced.Routing.Send("OrderQueue", message);
            }
        }
        
        public void Dispose()
        {
            _bus?.Dispose();
        }
    }
}