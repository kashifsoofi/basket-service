using System;
using System.Collections.Generic;
using Checkout.BasketService.Controllers;
using Checkout.BasketService.Models;
using Checkout.BasketService.Models.Requests;
using Checkout.BasketService.Services;
using Moq;
using Xunit;

namespace Checkout.BasketService.UnitTests.Controllers
{
    public class BasketItemsControllerTests
    {
        private readonly BasketItemsController _sut;

        private readonly Mock<IBasketService> _mockBasketService;

        private readonly string _testCustomerId = "1";

        public BasketItemsControllerTests()
        {
            _mockBasketService = new Mock<IBasketService>();
            _sut = new BasketItemsController(_mockBasketService.Object);
        }

        [Fact]
        public void AddItem_should_add_item_to_service()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketService.Setup(x => x.AddItem(_testCustomerId, It.IsAny<Item>()))
                .Returns(basket);

            var result = _sut.AddItem(_testCustomerId, new AddItemRequest 
            {
                ItemId = "Item1",
                Quantity = 1
            });

            _mockBasketService.Verify(x => x.AddItem(_testCustomerId, It.Is<Item>(i => i.ItemId == "Item1")), Times.Once);
        }

        [Fact]
        public void ChangeItemQuantity_should_update_item_quantity_in_service()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketService.Setup(x => x.ChangeItemQuantity(_testCustomerId, "Item1", 5))
                .Returns(basket);

            var result = _sut.ChangeItemQuantity(_testCustomerId, new ChangeItemQuantityRequest
            {
                ItemId = "Item1",
                NewQuantity = 5
            });

            _mockBasketService.Verify(x => x.ChangeItemQuantity(_testCustomerId, "Item1", 5), Times.Once);
        }

        [Fact]
        public void RemoveItem_should_remove_item_from_service()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketService.Setup(x => x.RemoveItem(_testCustomerId, "Item1"))
                .Returns(basket);

            var result = _sut.RemoveItem(_testCustomerId, "Item1");

            _mockBasketService.Verify(x => x.RemoveItem(_testCustomerId, "Item1"), Times.Once);
        }

        [Fact]
        public void RemoveAllItems_should_clear_basket_from_service()
        {
            var basket = new Basket(_testCustomerId, new List<Item>());
            _mockBasketService.Setup(x => x.ClearBasket(_testCustomerId))
                .Returns(basket);

            var result = _sut.RemoveAllItems(_testCustomerId);

            _mockBasketService.Verify(x => x.ClearBasket(_testCustomerId), Times.Once);
        }
    }
}
