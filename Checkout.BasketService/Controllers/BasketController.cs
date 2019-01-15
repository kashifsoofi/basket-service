using System.Net;
using Checkout.BasketService.Services;
using Checkout.BasketService.Services.Results;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.BasketService.Controllers
{
    [Route("api/{customerId}/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public IActionResult Get(string customerId)
        {
            var basket = _basketService.GetBasketByCustomerId(customerId);
            if (basket == null)
            {
                return NotFound();
            }

            return new OkObjectResult(basket);
        }

        [HttpPost]
        public IActionResult Create(string customerId)
        {
            var result = _basketService.CreateBasketForCustomer(customerId);
            if (result == CreateBasketResult.AlreadyExists)
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }

            var location = Url.Action("Get", "Basket", new { customerId }, Url.ActionContext.HttpContext.Request.Scheme);
            return Created(location, customerId);
        }
    }
}
