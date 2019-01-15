using System;
using System.Collections.Generic;
using Checkout.BasketService.Models;
using Checkout.BasketService.Services;
using Checkout.BasketService.Services.Results;
using Checkout.BasketService.Stores;
using FluentAssertions;
using Moq;
using Xunit;

namespace Checkout.BasketService.UnitTests.Services
{
    public class BasketServiceTests
    {
        private readonly IBasketService _sut;

        private readonly Mock<IBasketStore> _mockBasketStore;

        private readonly string _testCustomerId = "1";

        public BasketServiceTests()
        {
            _mockBasketStore = new Mock<IBasketStore>();
            _sut = new CustomerBasketService(_mockBasketStore.Object);
        }

        [Fact]
        public void FindByCustomerId_should_return_null_if_basket_not_in_store()
        {
            _mockBasketStore.Setup(x => x.FindByCustomerId(_testCustomerId))
                .Returns<Basket>(null);

            var basket = _sut.GetBasketByCustomerId(_testCustomerId);

            basket.Should().BeNull();
            _mockBasketStore.Verify(x => x.FindByCustomerId(_testCustomerId), Times.Once);
        }

        [Fact]
        public void FindByCustomerId_should_return_basket_if_in_store()
        {
            _mockBasketStore.Setup(x => x.FindByCustomerId(_testCustomerId))
                .Returns(new Basket(_testCustomerId, new List<Item>()));

            var basket = _sut.GetBasketByCustomerId(_testCustomerId);

            basket.Should().NotBeNull();
            basket.CustomerId.Should().Be(_testCustomerId);
            _mockBasketStore.Verify(x => x.FindByCustomerId(_testCustomerId), Times.Once);
        }

        [Fact]
        public void CreateBasketForCustomer_should_return_already_exists_if_basket_alredy_exists()
        {
            _mockBasketStore.Setup(x => x.FindByCustomerId(_testCustomerId))
                .Returns(new Basket(_testCustomerId, new List<Item>()));

            var result = _sut.CreateBasketForCustomer(_testCustomerId);

            result.Should().Be(CreateBasketResult.AlreadyExists);
            _mockBasketStore.Verify(x => x.FindByCustomerId(_testCustomerId), Times.Once);
            _mockBasketStore.Verify(x => x.Create(_testCustomerId), Times.Never);
        }

        [Fact]
        public void CreateBasketForCustomer_should_store_basket_and_return_created()
        {
            _mockBasketStore.Setup(x => x.FindByCustomerId(_testCustomerId))
                .Returns<Basket>(null);
            _mockBasketStore.Setup(x => x.Create(_testCustomerId))
                .Verifiable();

            var result = _sut.CreateBasketForCustomer(_testCustomerId);

            result.Should().Be(CreateBasketResult.Created);
            _mockBasketStore.Verify(x => x.FindByCustomerId(_testCustomerId), Times.Once);
            _mockBasketStore.Verify(x => x.Create(_testCustomerId), Times.Once);
        }
    }
}
