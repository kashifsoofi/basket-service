using System;
using System.Collections.Generic;
using System.Linq;
using Checkout.BasketService.Models;
using Checkout.BasketService.Stores;
using Checkout.BasketService.Stores.InMemory;
using FluentAssertions;
using Xunit;

namespace Checkout.BasketService.UnitTests.Stores
{
    public class InMemoryBasketStoreTests
    {
        private readonly IBasketStore _sut;

        private readonly List<Basket> _baskets;
        private readonly string _testCustomerId = "1";

        public InMemoryBasketStoreTests()
        {
            _baskets = new List<Basket>();
            _sut = new InMemoryBasketStore(_baskets);
        }

        [Fact]
        public void FindByCustomerId_should_return_null_if_basket_not_found()
        {
            var basket = _sut.FindByCustomerId(_testCustomerId);

            basket.Should().BeNull();
        }

        [Fact]
        public void FindByCustomerId_should_return_basket_if_found()
        {
            _baskets.Add(new Basket(_testCustomerId, new List<Item>()));

            var basket = _sut.FindByCustomerId(_testCustomerId);

            basket.Should().NotBeNull();
            basket.CustomerId.Should().Be(_testCustomerId);
        }

        [Fact]
        public void Create_should_save_basket()
        {
            _sut.Create(_testCustomerId);

            _baskets.Count.Should().Be(1);
            _baskets.First().CustomerId.Should().Be(_testCustomerId);
        }

        [Fact]
        public void Create_should_not_add_new_basket_if_already_exist_for_customer()
        {
            _baskets.Add(new Basket(_testCustomerId, new List<Item>()));
            _baskets.Count.Should().Be(1);

            _sut.Create(_testCustomerId);

            _baskets.Count.Should().Be(1);
        }

        [Fact]
        public void AddItem_should_add_new_item_to_basket_if_does_not_exist()
        {
            _baskets.Add(new Basket(_testCustomerId, new List<Item>()));

            _sut.AddItem(_testCustomerId, new Item("Item1", 1));

            _baskets.First().Items.Count.Should().Be(1);
            _baskets.First().Items.First().ItemId.Should().Be("Item1");
            _baskets.First().Items.First().Quantity.Should().Be(1);
        }

        [Fact]
        public void AddItem_should_not_add_new_item_to_basket_if_exists()
        {
            _baskets.Add(new Basket(_testCustomerId,
                new List<Item>
                {
                    new Item("Item1", 3)
                }));

            _sut.AddItem(_testCustomerId, new Item("Item1", 1));

            _baskets.First().Items.Count.Should().Be(1);
            _baskets.First().Items.First().ItemId.Should().Be("Item1");
            _baskets.First().Items.First().Quantity.Should().Be(3);
        }

        [Theory]
        [InlineData(2, 3, 3)]
        [InlineData(0, 5, 5)]
        [InlineData(2, 0, 0)]
        [InlineData(3, -5, 0)]
        public void UpdateItemQuantity_should_update_item_quantity(int initialQuantity, int newQuantity, int expected)
        {
            _baskets.Add(new Basket(_testCustomerId,
                new List<Item>
                {
                    new Item("Item1", initialQuantity)
                }));

            _sut.ChangeItemQuantity(_testCustomerId, "Item1", newQuantity);

            _baskets.First().Items.Count.Should().Be(1);
            _baskets.First().Items.First().ItemId.Should().Be("Item1");
            _baskets.First().Items.First().Quantity.Should().Be(expected);
        }

        [Fact]
        public void RemoveItem_should_remove_item_from_basket()
        {
            _baskets.Add(new Basket(_testCustomerId,
                new List<Item>
                {
                    new Item("Item1", 2)
                }));

            _sut.RemoveItem(_testCustomerId, "Item1");

            _baskets.First().Items.Count.Should().Be(0);
        }

        [Fact]
        public void ClearBasket_should_remove_all_items_from_basket()
        {
            _baskets.Add(new Basket(_testCustomerId,
                new List<Item>
                {
                    new Item("Item1", 1),
                    new Item("Item2", 2),
                    new Item("Item3", 3)
                }));

            _sut.ClearBasket(_testCustomerId);

            _baskets.First().Items.Count.Should().Be(0);
        }
    }
}
