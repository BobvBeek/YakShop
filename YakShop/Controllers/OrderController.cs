﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.DB;
using YakShop.Entities;
using YakShop.Models;
using YakShop.Services;
using YakShop.Repositories.Interfaces;
using YakShop.Mappers;
using YakShop.DTOs;

namespace YakShop.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IStockRepository _stockRepo;

        public OrderController(IOrderRepository orderRepo, IStockRepository stockRepo)
        {
            _orderRepo = orderRepo;
            _stockRepo = stockRepo;
        }

        //Places an order for milk and/or skins.
        //If the requested amount is available, it delivers the full amount; otherwise, it delivers what is available.
        //Updates Stock accordingly.
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto request, [FromServices] OrderChecker orderChecker)
        {
            //Get the current stock
            var stock = await _stockRepo.GetStockAsync();

            //Check the validity of the request and create an order
            var order = await orderChecker.CheckOrder(request, stock);

            //add the order to the repository
            await _orderRepo.AddOrderAsync(order);

            //Return the order with the status code indicating the delivery status
            return await orderChecker.CheckStatus(order);
        }
    }
}
