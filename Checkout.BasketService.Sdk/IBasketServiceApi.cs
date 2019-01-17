using System.Threading.Tasks;
using Checkout.BasketService.Sdk.Models;

namespace Checkout.BasketService.Sdk
{
    public interface IBasketServiceApi
    {
        Task<Basket> CreateBasketAsync(string customerId);
        Task<Basket> GetBasketAsync(string customerId);
        Task<Basket> AddItem(string customerId, Item item);
        Task<Basket> ChangeItemQuantity(string customerId, string itemId, int newQuantity);
        Task<Basket> RemoveItem(string customerId, string itemId);
        Task<Basket> ClearBasket(string customerId);
    }
}
