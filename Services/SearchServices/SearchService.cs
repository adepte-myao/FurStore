using Common.Extensions;
using FurStore.Common.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FurStore.Services.SearchServices
{
    public class SearchService : ISearchService
    {
        public List<SelectListItem> GetFurnitureTypesForSearchSelectList()
        {
            List<SelectListItem> furnitureTypesSLI = new List<SelectListItem>();
            foreach (FurnitureTypes item in Enum.GetValues(typeof(FurnitureTypes)))
            {
                furnitureTypesSLI.Add(new SelectListItem
                {
                    Value = item.ToString(),
                    Text = item.GetAttribute<DisplayAttribute>().Name
                });
            }
            furnitureTypesSLI.Insert(0, new SelectListItem
            {
                Value = "All",
                Text = "Все"
            });

            return furnitureTypesSLI;
        }

        public List<SelectListItem> GetFurnitureTypesSelectList()
        {
            List<SelectListItem> furnitureTypesSLI = new List<SelectListItem>();
            foreach (FurnitureTypes item in Enum.GetValues(typeof(FurnitureTypes)))
            {
                furnitureTypesSLI.Add(new SelectListItem
                {
                    Value = item.ToString(),
                    Text = item.GetAttribute<DisplayAttribute>().Name
                });
            }

            return furnitureTypesSLI;
        }
    }
}
