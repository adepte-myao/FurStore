using AutoMapper;
using FurStore.Common.Enums;
using FurStore.Data.ServicesBase;
using FurStore.Mappings;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurStore.Models.Store
{
    public class Furniture : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Название")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Ссылка на картинку")]
        public string ImageUrl { get; set; }

        [Required]
        [DisplayName("Цена в рублях")]
        public double Price { get; set; }

        [Required]
        [DisplayName("Тип мебели")]
        public FurnitureTypes FurnitureType { get; set; }

        public FurnitureOrderElement AsFOE(Guid orderId, int amount)
        {
            var foe = new FurnitureOrderElement
            {
                FurnitureId = Id,
                OrderId = orderId,
                Title = Title,
                Description = Description,
                ImageUrl = ImageUrl,
                Price = Price,
                FurnitureType = FurnitureType,
                Amount = amount
            };
            return foe;
        }
    }
}
