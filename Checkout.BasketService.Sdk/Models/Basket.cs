using System.Collections.Generic;

namespace Checkout.BasketService.Sdk.Models
{
    public class Basket
    {
        public string CustomerId { get; set; }
        public List<Item> Items { get; set; }
    }
}
