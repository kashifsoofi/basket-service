using System.ComponentModel.DataAnnotations;

namespace Checkout.BasketService.Models.Requests
{
    public class AddItemRequest
    {
        [Required]
        public string ItemId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
