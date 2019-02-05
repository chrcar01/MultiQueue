using MultiQueue.Core.Messages;
using MultiQueue.Core.Models.Shared;
using MultiQueue.Core.Queues;
using MultiQueue.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace MultiQueue.Core.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _repo;
        private readonly PaymentQueue _queue;
        
        public PaymentsService(IPaymentsRepository repo, PaymentQueue queue)
        {
            _repo = repo;
            _queue = queue;
        }

        public Task SayHello(Payment payment)
        {
            Console.WriteLine($"Thank you for your payment {payment.Id} of {payment.Amount:C}");
            return Task.CompletedTask;
        }

        public async Task<ServiceResponse<Payment>> Submit(Payment payment)
        {
            var result = new ServiceResponse<Payment>();
            try
            {
                var savePaymentTask = _repo.SavePayment(payment);
                var submitPaymentTask = _queue.Send(payment);
                await Task.WhenAll(savePaymentTask, submitPaymentTask);
                result.Data = savePaymentTask.Result;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;

        }
    }
}
