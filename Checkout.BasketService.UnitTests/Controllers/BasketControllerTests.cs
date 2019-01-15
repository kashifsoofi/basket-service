using System.Collections.Generic;
using System.Net;
using Checkout.BasketService.Controllers;
using Checkout.BasketService.Models;
using Checkout.BasketService.Services;
using Checkout.BasketService.Services.Results;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Xunit;

namespace Checkout.BasketService.UnitTests.Controllers
{
    public class BasketControllerTests
    {
        private readonly BasketController _sut;

        private readonly Mock<IBasketService> _mockBasketService;

        private readonly string _testCustomerId = "1";

        public BasketControllerTests()
        {
            _mockBasketService = new Mock<IBasketService>();
            _sut = new BasketController(_mockBasketService.Object);
        }

        [Fact]
        public void Get_should_return_not_found_if_basket_does_not_exist()
        {
            _mockBasketService.Setup(x => x.GetBasketByCustomerId(_testCustomerId))
                .Returns<Basket>(null);

            var result = _sut.Get(_testCustomerId);

            var notFoundResult = result as NotFoundResult;
            notFoundResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            _mockBasketService.Verify(x => x.GetBasketByCustomerId(_testCustomerId), Times.Once);
        }

        [Fact]
        public void Get_should_return_basket_if_basket_exists()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketService.Setup(x => x.GetBasketByCustomerId(_testCustomerId))
                .Returns(basket);

            var result = _sut.Get(_testCustomerId);

            var okObjectResult = result as OkObjectResult;
            okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var customerBasket = okObjectResult.Value as Basket;
            customerBasket?.CustomerId.Should().Be(_testCustomerId);
            _mockBasketService.Verify(x => x.GetBasketByCustomerId(_testCustomerId), Times.Once);
        }

        [Fact]
        public void Create_should_return_created_on_success()
        {
            _mockBasketService.Setup(x => x.CreateBasketForCustomer(_testCustomerId))
                .Returns(CreateBasketResult.Created);

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Scheme).Returns("http");

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

            var fakeActionContext = new ActionContext()
            {
                HttpContext = mockHttpContext.Object
            };

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.ActionContext).Returns(fakeActionContext);
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("");

            _sut.Url = mockUrlHelper.Object;

            var result = _sut.Create(_testCustomerId);

            var createdResult = result as CreatedResult;
            createdResult?.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var customerBasket = createdResult.Value as Basket;
            customerBasket?.CustomerId.Should().Be(_testCustomerId);
            _mockBasketService.Verify(x => x.CreateBasketForCustomer(_testCustomerId), Times.Once);
        }

        [Fact]
        public void Create_should_return_conflict_if_basket_already_exists()
        {
            _mockBasketService.Setup(x => x.CreateBasketForCustomer(_testCustomerId))
                .Returns(CreateBasketResult.AlreadyExists);

            var result = _sut.Create(_testCustomerId);

            var conflictResult = result as StatusCodeResult;
            conflictResult?.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
            _mockBasketService.Verify(x => x.CreateBasketForCustomer(_testCustomerId), Times.Once);
        }
    }
}
