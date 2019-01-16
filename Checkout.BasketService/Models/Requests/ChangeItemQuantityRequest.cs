namespace Checkout.BasketService.Models.Requests
{
    public class ChangeItemQuantityRequest
    {
        public string ItemId { get; set; }
        public int NewQuantity { get; set; }
    }
}
