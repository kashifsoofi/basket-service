using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.BasketService.Models;
using Checkout.BasketService.Models.Requests;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Checkout.BasketService.IntegrationTests.Controllers
{
    public class BasketItemsControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public BasketItemsControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AddItem_should_add_item()
        {
            var customerId = "ExistingTestClientId";
            var requestContent = new StringContent(JsonConvert.SerializeObject(new AddItemRequest { ItemId = "Item1", Quantity = 1 }), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PostAsync($"api/{customerId}/basket/items", requestContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();

            var basket = JsonConvert.DeserializeObject<Basket>(responseContent);
            basket.CustomerId.Should().Be(customerId);
            var item = basket.Items.FirstOrDefault(x => x.ItemId == "Item1");
            item.Should().NotBeNull();
            item.Quantity.Should().Be(1);
        }

        [Fact]
        public async Task ChangeItemQuantity_should_update_item_quantity()
        {
            var customerId = "TestClientIdWithSingleItem";
            var itemId = "Item1";
            var requestContent = new StringContent(JsonConvert.SerializeObject(new ChangeQuantityRequest { NewQuantity = 1 }), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PutAsync($"api/{customerId}/basket/items/{itemId}", requestContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();

            var basket = JsonConvert.DeserializeObject<Basket>(responseContent);
            var item = basket.Items.FirstOrDefault(x => x.ItemId == itemId);
            item?.Quantity.Should().Be(1);

            var storeBasket = _fixture.Baskets.FirstOrDefault(x => x.CustomerId == customerId);
            var storeItem = storeBasket.Items.FirstOrDefault(x => x.ItemId == itemId);
            storeItem?.Quantity.Should().Be(1);
        }

        [Fact]
        public async Task ChangeItemQuantity_should_add_item()
        {
            var customerId = "TestClientIdWithSingleItem";
            var itemId = "Item2";
            var requestContent = new StringContent(JsonConvert.SerializeObject(new ChangeQuantityRequest { NewQuantity = 2 }), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PutAsync($"api/{customerId}/basket/items/{itemId}", requestContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();

            var basket = JsonConvert.DeserializeObject<Basket>(responseContent);
            var item = basket.Items.FirstOrDefault(x => x.ItemId == itemId);
            item.Should().NotBeNull();
            item?.Quantity.Should().Be(2);

            var storeBasket = _fixture.Baskets.FirstOrDefault(x => x.CustomerId == customerId);
            var storeItem = storeBasket.Items.FirstOrDefault(x => x.ItemId == itemId);
            storeItem.Should().NotBeNull();
            storeItem?.Quantity.Should().Be(2);
        }

        [Fact]
        public async Task RemoveItem_should_remove_item_single_item()
        {
            var customerId = "TestClientIdWithMulitpleItems1";
            var itemId = "Item2";
            var response = await _fixture.Client.DeleteAsync($"api/{customerId}/basket/items/{itemId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();

            var basket = JsonConvert.DeserializeObject<Basket>(responseContent);
            basket.Items.Count.Should().Be(2);

            var storeBasket = _fixture.Baskets.FirstOrDefault(x => x.CustomerId == customerId);
            storeBasket.Items.Count.Should().Be(2);
            storeBasket.Items.FirstOrDefault(x => x.ItemId == itemId).Should().BeNull();
        }

        [Fact]
        public async Task RemoveAllItems_should_clear_basket()
        {
            var customerId = "TestClientIdWithMulitpleItems2";
            var response = await _fixture.Client.DeleteAsync($"api/{customerId}/basket/items");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();

            var basket = JsonConvert.DeserializeObject<Basket>(responseContent);
            basket.Items.Count.Should().Be(0);

            var storeBasket = _fixture.Baskets.FirstOrDefault(x => x.CustomerId == customerId);
            storeBasket.Items.Count.Should().Be(0);
        }
    }
}
