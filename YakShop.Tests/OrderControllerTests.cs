using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YakShop.Api.Entities;
using YakShop.Api.Models;
using YakShop.Api.DB;
using YakShop.Controllers;
using System.Threading.Tasks;
using Xunit;



namespace YakShop.Tests
{
    public class OrderControllerTests
    {
        // Helper method to create a YakDbContext with predefined stock
        private YakDbContext CreateContextWithStock(double milk, int skins)
        {
            var options = new DbContextOptionsBuilder<YakDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new YakDbContext(options);
            context.Stock.Add(new Stock
            {
                Milk = milk,
                Skins = skins,
                Day = 13,
                LastUpdate = System.DateTime.Now
            });
            context.SaveChanges();

            return context;
        }

        // Test for placing an order with sufficient stock
        [Fact]
        public async Task PlaceOrder_ReturnsCreated_WhenStockIsSufficient()
        {
            // Create database context with initial stock
            var context = CreateContextWithStock(milk: 2000, skins: 5);
            var controller = new OrderController(context);

            var order = new PlaceOrderRequest
            {
                Customer = "Alice",
                Order = new OrderContent { Milk = 1000, Skins = 2 }
            };

            var result = await controller.PlaceOrder(order);

            // Test for: Result responses, milk and skins quantities
            var created = Assert.IsType<CreatedResult>(result);
            var response = Assert.IsType<OrderContent>(created.Value);
            Assert.Equal(1000, response.Milk);
            Assert.Equal(2, response.Skins);
        }

        // Test for placing an order with partly insufficient stock
        [Fact]
        public async Task PlaceOrder_ReturnsPartialContent_WhenOnlySkinsAvailable()
        {
            var context = CreateContextWithStock(milk: 100, skins: 5);
            var controller = new OrderController(context);

            var order = new PlaceOrderRequest
            {
                Customer = "Bob",
                Order = new OrderContent { Milk = 500, Skins = 3 }
            };

            var result = await controller.PlaceOrder(order);

            // Test for: Result responses, skins quantity, and milk being null
            var partial = Assert.IsType<ObjectResult>(result);
            Assert.Equal(206, partial.StatusCode);
            var response = Assert.IsType<OrderContent>(partial.Value);
            Assert.Null(response.Milk);
            Assert.Equal(3, response.Skins);
        }

        // Test for placing an order when no stock is available
        [Fact]
        public async Task PlaceOrder_ReturnsNotFound_WhenNothingIsAvailable()
        {
            var context = CreateContextWithStock(milk: 0, skins: 0);
            var controller = new OrderController(context);

            var order = new PlaceOrderRequest
            {
                Customer = "Bob",
                Order = new OrderContent { Milk = 10, Skins = 1 }
            };

            var result = await controller.PlaceOrder(order);

            // Test for: Result responses
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
