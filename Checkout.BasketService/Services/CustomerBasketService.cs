using Checkout.BasketService.Models;
using Checkout.BasketService.Services.Results;
using Checkout.BasketService.Stores;

namespace Checkout.BasketService.Services
{
    public class CustomerBasketService : IBasketService
    {
        private readonly IBasketStore _basketStore;

        public CustomerBasketService(IBasketStore basketStore)
        {
            _basketStore = basketStore;
        }

        public Basket GetBasketByCustomerId(string customerId)
        {
            return _basketStore.FindByCustomerId(customerId);
        }

        public CreateBasketResult CreateBasketForCustomer(string customerId)
        {
            var basket = GetBasketByCustomerId(customerId);
            if (basket != null)
            {
                return CreateBasketResult.AlreadyExists;
            }

            _basketStore.Create(customerId);
            return CreateBasketResult.Created;
        }

        public Basket AddItem(string customerId, Item item)
        {
            _basketStore.AddItem(customerId, item);
            return GetBasketByCustomerId(customerId);
        }

        public Basket ChangeItemQuantity(string customerId, string itemId, int newQuantity)
        {
            _basketStore.ChangeItemQuantity(customerId, itemId, newQuantity);
            return GetBasketByCustomerId(customerId);
        }

        public Basket RemoveItem(string customerId, string itemId)
        {
            _basketStore.RemoveItem(customerId, itemId);
            return GetBasketByCustomerId(customerId);
        }

        public Basket ClearBasket(string customerId)
        {
            _basketStore.ClearBasket(customerId);
            return GetBasketByCustomerId(customerId);
        }
    }
}
