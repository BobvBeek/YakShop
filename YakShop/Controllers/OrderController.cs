using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.Api.DB;
using YakShop.Api.Entities;
using YakShop.Api.Models;
using YakShop.Api.Services;

namespace YakShop.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly YakDbContext _context;

        public OrderController(YakDbContext context)
        {
            _context = context;
        }

        //Places an order for milk and/or skins.
        //If the requested amount is available, it delivers the full amount; otherwise, it delivers what is available.
        //Updates Stock accordingly.
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync();

            if (stock == null)
            {
                return NotFound("There is no stock.");
            }

            //Get order details from the request
            var requestedMilk = request.Order.Milk ?? 0;
            var requestedSkins = request.Order.Skins ?? 0;

            //Calculate how much can be delivered based on the stock
            var deliverMilk = requestedMilk <= stock.Milk ? requestedMilk : 0;
            var deliverSkins = requestedSkins <= stock.Skins ? requestedSkins : 0;

            //If both items are out stock, return NotFound
            if (deliverMilk == 0 && deliverSkins == 0)
            {
                return NotFound();
            }

            //Update the live stock
            stock.Milk -= deliverMilk;
            stock.Skins -= deliverSkins;

            var order = new Order
            {
                Customer = request.Customer,
                Day = stock.Day,
                MilkOrdered = requestedMilk,
                SkinsOrdered = requestedSkins,
                MilkDelivered = deliverMilk > 0 ? deliverMilk : (double?)null,
                SkinsDelivered = deliverSkins > 0 ? deliverSkins : (int?)null
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var response = new OrderContent();
            if (deliverMilk > 0)
            {
                response.Milk = deliverMilk;
            }
            if (deliverSkins > 0)
            {
                response.Skins = deliverSkins;
            }

            //return 201 if full order is delivered, 206 if partial
            if (deliverMilk == requestedMilk && deliverSkins == requestedSkins)
            {
                return Created(string.Empty, response); ;
            }

            return StatusCode(206, response);
        }
    }
}
