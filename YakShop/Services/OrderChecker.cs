using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using YakShop.Entities;
using YakShop.Models;
using YakShop.Repositories.Interfaces;
using YakShop.Repositories;
using YakShop.Mappers;
using YakShop.DTOs;

namespace YakShop.Services
{
    public class OrderChecker
    {
        private readonly IStockRepository _stockRepo;

        public OrderChecker(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task<Order> CheckOrder(OrderDto orderRequest, Stock stock)
        {
            //Get order details from the request
            var requestedMilk = orderRequest.MilkOrdered ?? 0;
            var requestedSkins = orderRequest.SkinsOrdered ?? 0;

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
            return order switch
            {
                { MilkDelivered: > 0, SkinsDelivered: > 0 }
                    => new CreatedResult(string.Empty, OrderMapper.ToDto(order)),

                { MilkDelivered: > 0 } or { SkinsDelivered: > 0 }
                    => new ObjectResult(OrderMapper.ToDeliveredDto(order)) { StatusCode = StatusCodes.Status206PartialContent },

                _ => new NotFoundResult()
            };
        }
    }
}
