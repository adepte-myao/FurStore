using FurStore.Common.Enums;
using FurStore.Models.Store;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FurStore.ViewModels.Store
{
    public class FurnitureListVM
    {
        public IEnumerable<Furniture> Furnitures { get; set; } = new List<Furniture>();

        public PageVM PageViewModel { get; set; }
    }
}
