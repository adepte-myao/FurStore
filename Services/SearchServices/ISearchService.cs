using Microsoft.AspNetCore.Mvc.Rendering;

namespace FurStore.Services.SearchServices
{
    public interface ISearchService
    {
        // Added "All" item
        public List<SelectListItem> GetFurnitureTypesForSearchSelectList();

        public List<SelectListItem> GetFurnitureTypesSelectList();
    }
}
