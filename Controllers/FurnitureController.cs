using FurStore.Common.Enums;
using FurStore.Models.Auth;
using FurStore.Models.Store;
using FurStore.Services.FurnitureServices;
using FurStore.Services.OrderServices;
using FurStore.ViewModels.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FurStore.Controllers
{
    public class FurnitureController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IFurnitureService _furnitureService;
        private readonly IOrderService _orderService;
        private readonly int _pageSize;

        public FurnitureController(UserManager<User> userManager, 
            IFurnitureService furnitureService, IOrderService orderService)
        {
            _userManager = userManager;
            _furnitureService = furnitureService;
            _orderService = orderService;
            _pageSize = 12;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchString, string? furnitureType, int page = 1)
        {
            var furnituresList = await _furnitureService.GetBySearchStringAndFurnitureTypeAsync(searchString, furnitureType);

            var count = furnituresList.Count();
            var items = furnituresList.Skip((page - 1) * _pageSize).Take(_pageSize);

            PageVM pageViewModel = new PageVM(count, page, _pageSize);

            FurnitureListVM furnitureListVM = new FurnitureListVM()
            {
                Furnitures = items,
                PageViewModel = pageViewModel,
            };

            return View(furnitureListVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _furnitureService.GetByIdAsync(id);

            return View(item);
        }

        [Authorize(Roles = "Factory")]
        [HttpGet]
        public IActionResult CreateFurniture() 
        {
            ViewBag.DropDownValues = new SelectList(Enum.GetNames(typeof(FurnitureTypes)));

            return View(); 
        }

        [Authorize(Roles = "Factory")]
        [HttpPost]
        public async Task<IActionResult> CreateFurniture(Furniture furniture)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _furnitureService.CreateAsync(furniture);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Factory")]
        [HttpGet]
        public async Task<IActionResult> EditFurniture(Guid id)
        {
            var furniture = await _furnitureService.GetByIdAsync(id);

            if (furniture == null) return BadRequest();

            ViewBag.DropDownValues = new SelectList(Enum.GetNames(typeof(FurnitureTypes)));

            return View(furniture);
        }

        [Authorize(Roles = "Factory")]
        [HttpPost]
        public async Task<IActionResult> EditFurniture(Furniture furniture)
        {
            if (!ModelState.IsValid) return View();

            await _furnitureService.UpdateAsync(furniture);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Factory")]
        public async Task<IActionResult> DeleteFurniture(Guid id)
        {
            var furniture = await _furnitureService.GetByIdAsync(id);
            if (furniture == null) return BadRequest();

            await _furnitureService.DeleteAsync(furniture);

            return RedirectToAction(nameof(Index));
        }
    }
}
