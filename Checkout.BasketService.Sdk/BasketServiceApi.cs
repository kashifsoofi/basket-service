using System.Threading.Tasks;
using Checkout.BasketService.Sdk.Models;
using Checkout.BasketService.Sdk.Requests;

namespace Checkout.BasketService.Sdk
{
    public class BasketServiceApi : IBasketServiceApi
    {
        private readonly IApiClient _apiClient;

        public BasketServiceApi(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Basket> CreateBasketAsync(string customerId)
        {
            var path = $"api/{customerId}/basket";
            return await _apiClient.PostAsync<object, Basket>(path, null);
        }

        public async Task<Basket> GetBasketAsync(string customerId)
        {
            var path = $"api/{customerId}/basket";
            return await _apiClient.GetAsync<Basket>(path);
        }

        public async Task<Basket> AddItem(string customerId, Item item)
        {
            var path = $"api/{customerId}/basket/items";
            return await _apiClient.PostAsync<AddItemRequest, Basket>(path, new AddItemRequest { ItemId = item.ItemId, Quantity = item.Quantity });
        }

        public async Task<Basket> ChangeItemQuantity(string customerId, string itemId, int newQuantity)
        {
            var path = $"api/{customerId}/basket/items/{itemId}";
            return await _apiClient.PostAsync<ChangeQuantityRequest, Basket>(path, new ChangeQuantityRequest { NewQuantity = newQuantity });
        }

        public async Task<Basket> RemoveItem(string customerId, string itemId)
        {
            var path = $"api/{customerId}/basket/items/{itemId}";
            return await _apiClient.DeleteAsync<Basket>(path);
        }

        public async Task<Basket> ClearBasket(string customerId)
        {
            var path = $"api/{customerId}/basket/items";
            return await _apiClient.DeleteAsync<Basket>(path);
        }
    }
}
