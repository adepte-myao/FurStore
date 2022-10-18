using System.ComponentModel.DataAnnotations;

namespace FurStore.Common.Enums
{
    public enum FurnitureTypes
    {
        [Display(Name = "Мягкая мебель")]
        Soft = 1,

        [Display(Name = "Корпусная мебель")]
        Leather
    }
}
