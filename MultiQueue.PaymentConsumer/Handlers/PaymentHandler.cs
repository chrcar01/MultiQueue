using MultiQueue.Core.Messages;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using MultiQueue.Core.Services;

namespace MultiQueue.PaymentConsumer.Handlers
{
    public class PaymentHandler : IHandleMessages<Payment>
    {
        private readonly IPaymentsService _service;

        public PaymentHandler(IPaymentsService service)
        {
            _service = service;
        }
        
        public async Task Handle(Payment message)
        {
            await _service.SayHello(message);    
        }
    }
}
