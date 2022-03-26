using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events;
using Stock.API.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Consumer
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        readonly ISendEndpointProvider _sendEndpointProvider;
        readonly IPublishEndpoint _publishEndpoint;
        readonly ApplicationDbContext _applicationDbContext;
        public OrderCreatedEventConsumer(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint, ApplicationDbContext applicationDbContext)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
            _applicationDbContext = applicationDbContext;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();

           
          //  IMongoCollection<Models.Stock> collection = _mongoDbService.GetCollection<Models.Stock>();

            //Sipariş edilen ürünlerin stok miktarı sipariş adedinden fazla mı? değil mi?
           
                stockResult.Add( _applicationDbContext.Stocks.Any(s => s.ProductId == context.Message.ProductId && s.Count > context.Message.Count));

            //Eğer fazlaysa sipariş edilen ürünlerin stok miktarı güncelleniyor.
            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
              
                    Model.Stock stock = (_applicationDbContext.Stocks.FirstOrDefault(s => s.ProductId == context.Message.ProductId));
                    stock.Count -= context.Message.Count;


                    var updatedEntity = _applicationDbContext.Entry(stock);

                        updatedEntity.State = EntityState.Modified;

                    _applicationDbContext.SaveChanges();

                    ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
                    StockReservedEvent stockReservedEvent = new()
                    {
                        BuyerId = context.Message.BuyerId,
                        OrderId = context.Message.OrderId,
                        Count = context.Message.Count,
                        Price=context.Message.Price,
                        ProductId=context.Message.ProductId,
                        TotalPrice = context.Message.TotalPrice
                    };
                    await sendEndpoint.Send(stockReservedEvent);


                
            }
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Stok yetersiz..."
                };
                await _publishEndpoint.Publish(stockNotReservedEvent);
            }
        }
    }
}
