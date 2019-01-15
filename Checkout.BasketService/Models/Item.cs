namespace Checkout.BasketService.Models
{
    public class Item
    {
        public string ItemId { get; }
        public int Quantity { get; }

        public Item(string itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
