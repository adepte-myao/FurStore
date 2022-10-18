using FurStore.Common.Enums;
using FurStore.Data.ServicesBase;
using FurStore.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurStore.Models.Store
{
    public class Order : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public User? User { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreationTime { get; set; }
        
        public DateTime? DoneTime { get; set; }

        public List<FurnitureOrderElement> FurnitureElementsList { get; set; }

        [NotMapped]
        public double TotalPrice
        {
            get
            {
                double sum = 0;
                foreach (var item in FurnitureElementsList)
                {
                    sum += item.TotalPrice;
                }
                return sum;
            }

        }
    }
}
