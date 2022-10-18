using FurStore.Common.Enums;
using FurStore.Data;
using FurStore.Data.ServicesBase;
using FurStore.Models.Auth;
using FurStore.Models.Store;
using Microsoft.EntityFrameworkCore;

namespace FurStore.Services.OrderServices
{
    public class OrderService : EntityRepository<Order>, IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order?> GetOnCreatedOrderByUser(User user)
        {
            return await _context.Orders
                .Include(order => order.FurnitureElementsList)
                .Include(order => order.User)
                .FirstOrDefaultAsync(order =>
                    order.User == user &&
                    order.Status == OrderStatus.OnCreation);
        }

        public async Task<Order> CreateEmptyOrderAsync(User user)
        {
            var order = new Order
            {
                User = user,
                Status = OrderStatus.OnCreation,
                CreationTime = DateTime.Now,
                DoneTime = null,
                FurnitureElementsList = new List<FurnitureOrderElement>()
            };

            await CreateAsync(order);

            return order;
        }

        public async Task AddElementToOrderAsync(Furniture element, Order order)
        {
            var existing_foe = order.FurnitureElementsList.FirstOrDefault(foe => foe.FurnitureId == element.Id);
            if (existing_foe == null)
            {
                order.FurnitureElementsList.Add(element.AsFOE(orderId: order.Id, amount: 1));
            }
            else
            {
                existing_foe.Amount++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetSortedByCreationDateFullOrdersByUserAsync(User user)
        {
            return await _context.Orders
                .Include(order => order.User)
                .Include(order => order.FurnitureElementsList)
                .Where(order => order.User == user)
                .OrderByDescending(order => order.CreationTime)
                .ToListAsync();
        }

        public async Task<Order?> GetFullOrderByIdAndUserAsync(Guid orderId, User user)
        {
            return await _context.Orders
                .Include(order => order.User)
                .Include(order => order.FurnitureElementsList)
                .FirstOrDefaultAsync(order => order.Id == orderId && order.User == user);
        }

        public async Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus)
        {
            order.Status = orderStatus;
            await _context.SaveChangesAsync();
        }

        public async Task<FurnitureOrderElement?> GetFurnitureOrderElementAsync(Guid orderId, Guid furnitureId)
        {
            return await _context.FurnitureOrderElements
                .FirstOrDefaultAsync(item => item.FurnitureId == furnitureId && item.OrderId == orderId);
        }

        public async Task DeleteFurnitureOrderElementAsync(FurnitureOrderElement itemToRemove)
        {
            var order = await GetByIdAsync(itemToRemove.OrderId);
            order.FurnitureElementsList.Remove(itemToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFurnitureOrderElementAmountAsync(FurnitureOrderElement item, bool isPlus)
        {
            if (isPlus)
            {
                item.Amount++;
            }
            else
            {
                if (item.Amount > 1)
                    item.Amount -= 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersForFactoryAsync()
        {
            return await _context.Orders
                .Where(order => order.Status == OrderStatus.Created ||
                                order.Status == OrderStatus.Accepted ||
                                order.Status == OrderStatus.Processing)
                .Include(order => order.User)
                .OrderByDescending(order => order.CreationTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersDescendedAsync()
        {
            return await _context.Orders
                .Include(order => order.User)
                .OrderByDescending(order => order.CreationTime)
                .ToListAsync();
        }

        public async Task<Order?> GetFullOrderByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(order => order.User)
                .Include(order => order.FurnitureElementsList)
                .FirstOrDefaultAsync(order => order.Id == id);
        }
    }
}
