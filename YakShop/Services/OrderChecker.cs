using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using YakShop.Api.Entities;
using YakShop.Api.Models;
using YakShop.Models;
using YakShop.Repositories.Interfaces;
using YakShop.Repositories.Repositories;

namespace YakShop.Services
{
    public class OrderChecker
    {
        private readonly IStockRepository _stockRepo;

        public OrderChecker(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task<Order> CheckOrder(PlaceOrderRequest orderRequest, Stock stock)
        {
            //Get order details from the request
            var requestedMilk = orderRequest.Order.Milk ?? 0;
            var requestedSkins = orderRequest.Order.Skins ?? 0;

            var deliverMilk = requestedMilk <= stock.Milk ? requestedMilk : 0;
            var deliverSkins = requestedSkins <= stock.Skins ? requestedSkins : 0;

            //Update live stock
            stock.Milk -= requestedMilk <= stock.Milk ? requestedMilk : 0;
            stock.Skins -= requestedSkins <= stock.Skins ? requestedSkins : 0;
            await _stockRepo.UpdateStockAsync(stock);

            return new Order
            {
                Customer = orderRequest.Customer,
                Day = stock.Day,
                MilkOrdered = requestedMilk,
                SkinsOrdered = requestedSkins,
                MilkDelivered = deliverMilk > 0 ? deliverMilk : (double?)null,
                SkinsDelivered = deliverSkins > 0 ? deliverSkins : (int?)null
            };
        }

        public async Task<IActionResult> CheckStatus(Order order)
        {
            var content = new OrderContent
            {
                Milk = order.MilkDelivered,
                Skins = order.SkinsDelivered
            };

            return order switch
            {
                { MilkDelivered: > 0, SkinsDelivered: > 0 }
                    => new CreatedResult(string.Empty, content),

                { MilkDelivered: > 0 } or { SkinsDelivered: > 0 }
                    => new ObjectResult(content) { StatusCode = StatusCodes.Status206PartialContent },

                _ => new NotFoundResult()
            };
        }
    }
}
