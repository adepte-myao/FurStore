using FurStore.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurStore.Models.Store
{
    [Index("OrderId", "FurnitureId", IsUnique = true)]
    public class FurnitureOrderElement
    {
        public Guid FurnitureId { get; set; }

        public Guid OrderId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public FurnitureTypes FurnitureType { get; set; }

        [Required]
        public int Amount { get; set; }

        [NotMapped]
        public double TotalPrice
        {
            get => Amount * Price;
        }
    }
}
