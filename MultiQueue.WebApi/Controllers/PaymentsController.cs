using System;
using System.Threading.Tasks;
using System.Web.Http;
using MultiQueue.Core.Messages;
using MultiQueue.Core.Services;

namespace MultiQueue.WebApi.Controllers
{
    public class PaymentsController : ApiController
    {
        private readonly IPaymentsService _service;

        public PaymentsController(IPaymentsService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Submit(Payment payment)
        {
            var result = await _service.Submit(payment);
            return result.HasErrors 
                ? (IHttpActionResult) BadRequest(String.Join("|", result.ErrorMessages)) 
                : Ok(result.Data);
        }
    }
}
