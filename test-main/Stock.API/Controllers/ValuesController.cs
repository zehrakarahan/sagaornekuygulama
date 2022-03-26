using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly ApplicationDbContext _applicationDbContext;
        public ValuesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("UrunEkleme")]
        public IActionResult UrunEkleme()
        {
            Random rastgele = new Random();
            for (int i = 1; i < 10; i++)
            {
                _applicationDbContext.Stocks.Add(
                        new Model.Stock
                        {

                            ProductId = i,
                            Count = i * rastgele.Next(1, 20)

                        });
                _applicationDbContext.SaveChanges();
            }
            return Ok("test deneme");
        }
    }
}
