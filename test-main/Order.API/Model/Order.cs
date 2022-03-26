using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int BuyerId { get; set; }

       // public int OrderItemId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalPrice { get; set; }

        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
