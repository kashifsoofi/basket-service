using System.Collections.Generic;
using System.Linq;
using Checkout.BasketService.Models;

namespace Checkout.BasketService.Stores.InMemory
{
    public class InMemoryBasketStore : IBasketStore
    {
        private readonly List<Basket> _respository;

        public InMemoryBasketStore(List<Basket> baskets)
        {
            _respository = baskets;
        }

        public InMemoryBasketStore()
            : this(new List<Basket>())
        { }

        public Basket FindByCustomerId(string customerId)
        {
            return _respository.FirstOrDefault(x => x.CustomerId == customerId);
        }

        private Item FindItem(string customerId, string itemId)
        {
            return _respository
                .FirstOrDefault(x => x.CustomerId == customerId)
                ?.Items.FirstOrDefault(i => i.ItemId == itemId);
        }

        public void Create(string customerId)
        {
            var found = FindByCustomerId(customerId);
            if (found == null)
            {
                var basket = new Basket(customerId, new List<Item>());
                _respository.Add(basket);
            }
        }

        public void AddItem(string customerId, Item item)
        {
            var basket = FindByCustomerId(customerId);
            if (basket != null && !basket.Items.Any(x => x.ItemId == item.ItemId))
            {
                basket?.Items.Add(item);
            }
        }

        public void ChangeItemQuantity(string customerId, string itemId, int newQuantity)
        {
            if (newQuantity < 0)
                newQuantity = 0;

            var basket = FindByCustomerId(customerId);
            var existingItem = basket?.Items.FirstOrDefault(x => x.ItemId == itemId);
            if (existingItem != null)
            {
                basket.Items.Remove(existingItem);
            }

            var item = new Item(itemId, newQuantity);
            basket.Items.Add(item);
        }

        public void RemoveItem(string customerId, string itemId)
        {
            var basket = FindByCustomerId(customerId);
            var item = basket?.Items.FirstOrDefault(x => x.ItemId == itemId);
            if (item != null)
            {
                basket.Items.Remove(item);
            }
        }

        public void ClearBasket(string customerId)
        {
            var basket = FindByCustomerId(customerId);
            basket?.Items.Clear();
        }
    }
}
