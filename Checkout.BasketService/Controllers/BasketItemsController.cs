using Checkout.BasketService.Models;
using Checkout.BasketService.Models.Requests;
using Checkout.BasketService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.BasketService.Controllers
{
    [Route("api/{customerId}/[controller]")]
    public class BasketItemsController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketItemsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost]
        public IActionResult AddItem(string customerId, [FromBody] AddItemRequest request)
        {
            var basket = _basketService.AddItem(customerId, new Item(request.ItemId, request.Quantity));
            return new OkObjectResult(basket);
        }

        [HttpPut]
        public IActionResult ChangeItemQuantity(string customerId, [FromBody] ChangeItemQuantityRequest request)
        {
            var basket = _basketService.ChangeItemQuantity(customerId, request.ItemId, request.NewQuantity);
            return new OkObjectResult(basket);
        }

        [Route("{itemId}")]
        [HttpDelete]
        public IActionResult RemoveItem(string customerId, string itemId)
        {
            var basket = _basketService.RemoveItem(customerId, itemId);
            return new OkObjectResult(basket);
        }

        [HttpDelete]
        public IActionResult RemoveAllItems(string customerId)
        {
            var basket = _basketService.ClearBasket(customerId);
            return new OkObjectResult(basket);
        }
    }
}
