using FurStore.Common.Enums;
using FurStore.Data.ServicesBase;
using FurStore.Models.Auth;
using FurStore.Models.Store;

namespace FurStore.Services.OrderServices
{
    public interface IOrderService : IEntityRepository<Order>
    {
        public Task<Order?> GetFullOrderByIdAsync(Guid id);

        public Task<Order?> GetOnCreatedOrderByUser(User user);

        public Task<Order> CreateEmptyOrderAsync(User user);

        public Task AddElementToOrderAsync(Furniture element, Order order);

        // Full means each order includes its elemets
        public Task<IEnumerable<Order>> GetSortedByCreationDateFullOrdersByUserAsync(User user);

        // Full means each order includes its elemets
        public Task<Order?> GetFullOrderByIdAndUserAsync(Guid orderId, User user);

        public Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus);

        public Task<FurnitureOrderElement?> GetFurnitureOrderElementAsync(Guid orderId, Guid furnitureId);

        public Task DeleteFurnitureOrderElementAsync(FurnitureOrderElement itemToRemove);

        public Task UpdateFurnitureOrderElementAmountAsync(FurnitureOrderElement item, bool isPlus);

        public Task<IEnumerable<Order>> GetOrdersForFactoryAsync();

        public Task<IEnumerable<Order>> GetAllOrdersDescendedAsync();
    }
}
