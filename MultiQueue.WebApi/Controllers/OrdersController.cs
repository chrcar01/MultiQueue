using MultiQueue.Core.Messages;
using MultiQueue.Core.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace MultiQueue.WebApi.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly IOrdersService _service;

        public OrdersController(IOrdersService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Submit(Order order)
        {
            var result = await _service.Submit(order);
            return result.HasErrors
                ? (IHttpActionResult) BadRequest(String.Join("|", result.ErrorMessages))
                : Ok(result.Data);
        }
    }
}
