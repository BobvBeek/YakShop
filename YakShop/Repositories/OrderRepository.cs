using Microsoft.EntityFrameworkCore;
using YakShop.DB;
using YakShop.Entities;
using YakShop.Repositories.Interfaces;

namespace YakShop.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly YakDbContext _context;

        public OrderRepository(YakDbContext context)
        {
            _context = context;
        }

        // Method to add a new order
        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        // Method to update an order
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        // Method to get all orders
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        // Method to get an order by ID
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        // Method to delete an order by ID
        public async Task DeleteOrderAsync(int id)
        {
            var order = await GetOrderByIdAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        //Method to remove all orders
        public async Task RemoveAllOrders()
        {
            _context.Orders.RemoveRange(_context.Orders);
            await _context.SaveChangesAsync();
        }
    }
}
