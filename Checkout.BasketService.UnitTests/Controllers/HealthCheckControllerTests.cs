using System.Net;
using Checkout.BasketService.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Checkout.BasketService.UnitTests.Controllers
{
    public class HealthCheckControllerTests
    {
        private readonly HealthCheckController _sut;

        public HealthCheckControllerTests()
        {
            _sut = new HealthCheckController();
        }

        [Fact]
        public void Should_return_ok()
        {
            var result = _sut.Get();

            var okResult = result as OkResult;
            okResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
