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

        [Fact]
        public void AddItem_should_add_item_to_store()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketStore.Setup(x => x.FindByCustomerId(_testCustomerId))
                .Returns(basket);
            _mockBasketStore.Setup(x => x.AddItem(_testCustomerId, It.IsAny<Item>()))
                .Verifiable();

            var result = _sut.AddItem(_testCustomerId, new Item("item1", 1));

            _mockBasketStore.Verify(x => x.FindByCustomerId(_testCustomerId), Times.Once);
            _mockBasketStore.Verify(x => x.AddItem(_testCustomerId, It.IsAny<Item>()), Times.Once);
        }

        [Fact]
        public void ChangeItemQuantity_should_change_item_quantity_to_store()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketStore.Setup(x => x.FindByCustomerId(_testCustomerId))
                .Returns(basket);
            _mockBasketStore.Setup(x => x.ChangeItemQuantity(_testCustomerId, "item1", 5))
                .Verifiable();

            var result = _sut.ChangeItemQuantity(_testCustomerId, "item1", 5);

            _mockBasketStore.Verify(x => x.FindByCustomerId(_testCustomerId), Times.Once);
            _mockBasketStore.Verify(x => x.ChangeItemQuantity(_testCustomerId, "item1", 5), Times.Once);
        }

        [Fact]
        public void RemoveItem_should_remove_item_from_store()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketStore.Setup(x => x.FindByCustomerId(_testCustomerId))
                .Returns(basket);
            _mockBasketStore.Setup(x => x.RemoveItem(_testCustomerId, "item1"))
                .Verifiable();

            var result = _sut.RemoveItem(_testCustomerId, "item1");

            _mockBasketStore.Verify(x => x.FindByCustomerId(_testCustomerId), Times.Once);
            _mockBasketStore.Verify(x => x.RemoveItem(_testCustomerId, "item1"), Times.Once);
        }

        [Fact]
        public void ClearBasket_should_clear_items_from_store()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketStore.Setup(x => x.FindByCustomerId(_testCustomerId))
                .Returns(basket);
            _mockBasketStore.Setup(x => x.ClearBasket(_testCustomerId))
                .Verifiable();

            var result = _sut.ClearBasket(_testCustomerId);

            _mockBasketStore.Verify(x => x.FindByCustomerId(_testCustomerId), Times.Once);
            _mockBasketStore.Verify(x => x.ClearBasket(_testCustomerId), Times.Once);
        }
    }
}
