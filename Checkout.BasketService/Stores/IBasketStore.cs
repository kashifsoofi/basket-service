using Checkout.BasketService.Models;

namespace Checkout.BasketService.Stores
{
    public interface IBasketStore
    {
        Basket FindByCustomerId(string customerId);
        void Create(string customerId);
        void AddItem(string customerId, Item item);
        void UpdateItemQuantity(string customerId, string itemId, int difference);
        void RemoveItem(string customerId, string itemId);
        void ClearBasket(string customerId);
    }
}
