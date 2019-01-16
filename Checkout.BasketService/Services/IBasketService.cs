using Checkout.BasketService.Models;
using Checkout.BasketService.Services.Results;

namespace Checkout.BasketService.Services
{
    public interface IBasketService
    {
        Basket GetBasketByCustomerId(string customerId);
        CreateBasketResult CreateBasketForCustomer(string customerId);
        Basket AddItem(string customerId, Item item);
        Basket ChangeItemQuantity(string customerId, string itemId, int newQuantity);
        Basket RemoveItem(string customerId, string itemId);
        Basket ClearBasket(string customerId);
    }
}
