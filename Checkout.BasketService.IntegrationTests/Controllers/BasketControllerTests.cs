using System.Net;
using System.Threading.Tasks;
using Checkout.BasketService.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Checkout.BasketService.IntegrationTests.Controllers
{
    public class BasketControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public BasketControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Get_should_return_not_found_if_basket_does_not_exist()
        {
            var customerId = "DoesNotExistTestClientId";
            var response = await _fixture.Client.GetAsync($"api/{customerId}/basket");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_should_return_basket_if_basket_exists()
        {
            var customerId = "ExistingTestClientId";
            var response = await _fixture.Client.GetAsync($"api/{customerId}/basket");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();

            var basket = JsonConvert.DeserializeObject<Basket>(content);
            basket.CustomerId.Should().Be(customerId);
        }

        [Fact]
        public async Task Create_should_return_created_on_success()
        {
            var customerId = "TestClientIdToCreate";
            var response = await _fixture.Client.PostAsync($"api/{customerId}/basket", null);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Create_should_return_conflict_if_basket_already_exists()
        {
            string customerId = "ExistingTestClientId";
            var response = await _fixture.Client.PostAsync($"api/{customerId}/basket", null);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
    }
}
