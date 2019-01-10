using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Checkout.BasketService.IntegrationTests.Controllers
{
    public class HealthCheckControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public HealthCheckControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_receive_ok_when_I_get_healthcheck()
        {
            var response = await _fixture.Client.GetAsync("api/healthcheck");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
