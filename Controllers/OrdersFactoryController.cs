using FurStore.Common.Enums;
using FurStore.Data;
using FurStore.Models.Auth;
using FurStore.Services.FurnitureServices;
using FurStore.Services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurStore.Controllers
{
    [Authorize(Roles = "Factory")]
    public class OrdersFactoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFurnitureService _furnitureService;
        private readonly IOrderService _orderService;

        public OrdersFactoryController(AppDbContext context, IFurnitureService furnitureService,
            IOrderService orderService)
        {
            _context = context;
            _furnitureService = furnitureService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersForFactoryAsync();

            ViewBag.IsAll = false;
            return View(orders);
        }

        public async Task<IActionResult> AllOrders()
        {
            var orders = await _orderService.GetAllOrdersDescendedAsync();

            ViewBag.IsAll = true;
            return View(nameof(Index), orders);
        }

        public async Task<IActionResult> UpdateOrderStatus(Guid id, string status, bool isAll)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            try
            {
                Enum.TryParse(status, out OrderStatus result);
                await  _orderService.UpdateOrderStatusAsync(order, result);

                if (isAll)
                {
                    return RedirectToAction(nameof(AllOrders));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var order = await _orderService.GetFullOrderByIdAsync(id);

            if (order == null) return NotFound();

            var foes = order.FurnitureElementsList;

            return View(foes);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null) return BadRequest();

            await _orderService.DeleteAsync(order);

            return RedirectToAction(nameof(Index));
        }
    }
}
