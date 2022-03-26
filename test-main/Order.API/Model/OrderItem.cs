using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Model
{
    public class OrderItem
    { 
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
