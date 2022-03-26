using MassTransit;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Consumer
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async  Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            bool paymentState = true;

            //Ödeme başarılıysa
            if (paymentState)
            {
                Console.WriteLine("Ödeme başarılı...");
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };
                await _publishEndpoint.Publish(paymentCompletedEvent);
            }
            else
            {
                Console.WriteLine("Ödeme başarısız...");
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Count = context.Message.Count,
                    Price=context.Message.Price,
                    ProductId=context.Message.ProductId,
                    Message = "Bakiye yetersiz!"
                };
                await _publishEndpoint.Publish(paymentFailedEvent);
            }
        }
    }
}
