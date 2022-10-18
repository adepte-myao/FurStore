using System.ComponentModel.DataAnnotations;

namespace FurStore.Common.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Формируется")]
        OnCreation,         // When a client fills the cart

        [Display(Name = "Отправлен")]
        Created,            // When a client makes an order

        [Display(Name = "Принят")]
        Accepted,           // When a factory accepts the order

        [Display(Name = "В процессе производства")]
        Processing,         // When a factory starts creating required items

        [Display(Name = "Собран")]
        Made,               // When a factory finishes a creation

        [Display(Name = "Готов к выдаче")]
        DeliveredToStore,   // When an order is ready to be got by its client

        [Display(Name = "Завершен")]
        Taken,              // = order completed

        [Display(Name = "Отменен")]
        Cancelled,          // When a client cancels an order (doesn't want it anymore)

        [Display(Name = "Возврат")]
        Returned,           // When a client returns the order
    }
}
