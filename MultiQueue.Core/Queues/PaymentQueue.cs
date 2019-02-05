using Rebus.Bus;
using System;
using System.Threading.Tasks;

namespace MultiQueue.Core.Queues
{
    public class PaymentQueue : IDisposable
    {
        private readonly IBus _bus;
        
        public PaymentQueue(IBus bus)
        {
            _bus = bus;
        }

        public async Task Send(object message)
        {
            if (_bus != null)
            {
                await _bus.Advanced.Routing.Send("PaymentQueue", message);
            }
        }
        
        public void Dispose()
        {
            _bus?.Dispose();
        }
    }
}
