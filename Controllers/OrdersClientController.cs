using FurStore.Common.Enums;
using FurStore.Data;
using FurStore.Models.Auth;
using FurStore.Services.FurnitureServices;
using FurStore.Services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FurStore.Controllers
{
    [Authorize(Roles = "Client")]
    public class OrdersClientController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IFurnitureService _furnitureService;
        private readonly IOrderService _orderService;

        public OrdersClientController(UserManager<User> userManager,
            IFurnitureService furnitureService, IOrderService orderService)
        {
            _userManager = userManager;
            _furnitureService = furnitureService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var orders = await _orderService.GetSortedByCreationDateFullOrdersByUserAsync(user);

            return View(orders);
        }

        public async Task<IActionResult> AddItem(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var item = await _furnitureService.GetByIdAsync(id);
            if (item == null || user == null)
            {
                return NotFound();
            }

            var currentOrder = await _orderService.GetOnCreatedOrderByUser(user);
            if (currentOrder == null)
            {
                currentOrder = await _orderService.CreateEmptyOrderAsync(user);
            }

            await _orderService.AddElementToOrderAsync(item, currentOrder);

            return RedirectToAction(nameof(Index), "Furniture");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = await _orderService.GetFullOrderByIdAndUserAsync(id, user);

            if (order == null) return NotFound();

            var foes = order.FurnitureElementsList;

            return View(foes);
        }

        public async Task<IActionResult> MakeOrder(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = await _orderService.GetFullOrderByIdAndUserAsync(id, user);

            if (order == null) return BadRequest();

            await _orderService.UpdateOrderStatusAsync(order, OrderStatus.Created);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = await _orderService.GetFullOrderByIdAndUserAsync(id, user);

            if (order == null) return BadRequest();

            await _orderService.UpdateOrderStatusAsync(order, OrderStatus.Cancelled);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = await _orderService.GetFullOrderByIdAndUserAsync(id, user);

            if (order == null) return BadRequest();

            await _orderService.DeleteAsync(order);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem([FromQuery]Guid orderId, [FromQuery]Guid itemId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = await _orderService.GetFullOrderByIdAndUserAsync(orderId, user);

            if (order == null) return BadRequest();

            var itemToRemove = await _orderService.GetFurnitureOrderElementAsync(orderId, itemId);

            if (itemToRemove == null) return BadRequest();

            await _orderService.DeleteFurnitureOrderElementAsync(itemToRemove);

            if (order.FurnitureElementsList.Count == 0)
            {
                await _orderService.DeleteAsync(order);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Details), routeValues: new { id = orderId.ToString() });
        }

        public async Task<IActionResult> UpdateFoeAmount(Guid orderId, Guid furId, string option)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = await _orderService.GetFullOrderByIdAndUserAsync(orderId, user);
            
            if (order == null) return BadRequest();
            if (order.Status != OrderStatus.OnCreation)
            {
                return RedirectToAction(nameof(Details), routeValues: new { id = orderId }); ;
            }

            var item = order.FurnitureElementsList.FirstOrDefault(item => item.FurnitureId == furId);

            if (item == null || option == null) return BadRequest();

            await _orderService.UpdateFurnitureOrderElementAmountAsync(item, isPlus: option == "plus");

            return RedirectToAction(nameof(Details), routeValues: new { id = orderId });;
        }
    }
}
