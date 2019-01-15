using System.Collections.Generic;
using System.Linq;

namespace Checkout.BasketService.Models
{
    public class Basket
    {
        private readonly List<Item> _items;
        public string CustomerId { get; }

        public Basket(string customerId, List<Item> items)
        {
            CustomerId = customerId;
            _items = items.ToList();
        }

        public List<Item> Items
        {
            get { return _items; }
        }
    }
}
