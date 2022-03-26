using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Model
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }
   
        public int ProductId { get; set; }

        public int Count { get; set; }
  
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
