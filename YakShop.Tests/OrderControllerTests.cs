using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YakShop.Entities;
using YakShop.Models;
using YakShop.DB;
using YakShop.Controllers;
using System.Threading.Tasks;
using Xunit;
using YakShop.Repositories;
using YakShop.Services;
using YakShop.DTOs;



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
            var orderRepository = new OrderRepository(context);
            var stockRepository = new StockRepository(context);
            var orderChecker = new OrderChecker(stockRepository);

            // Create controller with repositories and service
            var controller = new OrderController(orderRepository, stockRepository);

            var order = new OrderDto
            {
                Customer = "Alice",
                MilkOrdered = 1000,
                SkinsOrdered = 2
            };

            var result = await controller.PlaceOrder(order, orderChecker);

            // Test for: Result responses, milk and skins quantities
            var created = Assert.IsType<CreatedResult>(result);
            var response = Assert.IsType<OrderDto>(created.Value);
            Assert.Equal(1000, response.MilkOrdered);
            Assert.Equal(2, response.SkinsOrdered);
        }

        // Test for placing an order with partly insufficient stock
        [Fact]
        public async Task PlaceOrder_ReturnsPartialContent_WhenOnlySkinsAvailable()
        {
            // Create database context with initial stock
            var context = CreateContextWithStock(milk: 2000, skins: 5);
            var orderRepository = new OrderRepository(context);
            var stockRepository = new StockRepository(context);
            var orderChecker = new OrderChecker(stockRepository);

            // Create controller with repositories and service
            var controller = new OrderController(orderRepository, stockRepository);

            var order = new OrderDto
            {
                Customer = "`Bob",
                MilkOrdered = 2500,
                SkinsOrdered = 3
            };

            var result = await controller.PlaceOrder(order, orderChecker);

            // Test for: Result responses, skins quantity, and milk being null
            var partial = Assert.IsType<ObjectResult>(result);
            Assert.Equal(206, partial.StatusCode);
            var response = Assert.IsType<OrderDto>(partial.Value);
            Assert.Null(response.MilkOrdered);
            Assert.Equal(3, response.SkinsOrdered);
        }

        // Test for placing an order when no stock is available
        [Fact]
        public async Task PlaceOrder_ReturnsNotFound_WhenNothingIsAvailable()
        {
            // Create database context with initial stock
            var context = CreateContextWithStock(milk: 5, skins: 0);
            var orderRepository = new OrderRepository(context);
            var stockRepository = new StockRepository(context);
            var orderChecker = new OrderChecker(stockRepository);

            // Create controller with repositories and service
            var controller = new OrderController(orderRepository, stockRepository);

            var order = new OrderDto
            {
                Customer = "Bob",
                MilkOrdered = 10,
                SkinsOrdered = 2
            };

            var result = await controller.PlaceOrder(order, orderChecker);

            // Test for: Result responses
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
