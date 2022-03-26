using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Model;
using Order.API.Model.Contexts;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly  ApplicationDbContext _applicationDbContext;
        readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(ApplicationDbContext applicationDbContext,IPublishEndpoint publishEndpoint)
        {
            _applicationDbContext = applicationDbContext;
            _publishEndpoint = publishEndpoint;
        }

        //[HttpGet("UrunEkleme")]
        //public IActionResult UrunEkleme()
        //{
        //    Random rastgele = new Random();
        //    for (int i = 1; i < 10; i++)
        //    {
        //        _applicationDbContext.Stocks.Add(
        //                new Model.Stock
        //                {

        //                    ProductId = i,
        //                    Count = i * rastgele.Next(1, 20)

        //                });
        //        _applicationDbContext.SaveChanges();
        //    }
        //    return Ok("test deneme");
        //}



        [HttpPost("CreateOrder")]
        public async  Task<IActionResult> CreateOrder(OrderVM model)
        {
            //Order.API.Model.Order order = new()
            //{
            //    BuyerId = model.BuyerId,
            //    OrderItems = model.OrderItems.Select(oi => new OrderItem
            //    {
            //        Count = oi.Count,
            //        Price = oi.Price,
            //        ProductId = oi.ProductId
            //    }).ToList(),
            //    OrderStatus = OrderStatus.Suspend,
            //    TotalPrice = model.OrderItems.Sum(oi => oi.Count * oi.Price),
            //    CreatedDate = DateTime.Now
            //};
            //var sonuc = new OrderItem
            //{

            //    ProductId = model.OrderItems.FirstOrDefault().ProductId,
            //    Count = model.OrderItems.FirstOrDefault().Count,
            //    Price = model.OrderItems.FirstOrDefault().Price

            //};
            //_applicationDbContext.OrderItems.Add(sonuc);
            //_applicationDbContext.SaveChanges();

            var order = new Model.Order
            {

                BuyerId = model.BuyerId,
                TotalPrice = model.OrderItems.Count * model.OrderItems.Price,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatus.Suspend,
                ProductId = model.OrderItems.ProductId,
                Count = model.OrderItems.Count,
                Price = model.OrderItems.Price

            };

            //_applicationDbContext.Orders.Add(order); 

            //_applicationDbContext.SaveChanges();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                OrderId = order.Id,
                BuyerId = order.BuyerId,
                TotalPrice = order.TotalPrice,
                Count = order.Count,
                Price=order.Price,
                ProductId=order.ProductId
            };
            await _publishEndpoint.Publish(orderCreatedEvent);
            return Ok(true);
        }

    }
}
