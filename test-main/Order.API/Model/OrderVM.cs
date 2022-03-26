using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Model
{
    public class OrderVM
    {
        public int BuyerId { get; set; }
        public OrderItemVM OrderItems { get; set; }
    }
}
