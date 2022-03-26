using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.API.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Consumer
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        readonly ApplicationDbContext _applicationDbContext;
        public PaymentFailedEventConsumer(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {

                Model.Stock stock = (_applicationDbContext.Stocks.FirstOrDefault(s => s.ProductId == context.Message.ProductId));
                if (stock != null)
                {
                 
                    stock.Count += context.Message.Count;
                    var updatedEntity = _applicationDbContext.Entry(stock);

                    updatedEntity.State = EntityState.Modified;

                    _applicationDbContext.SaveChanges();
                  
                }
        }
    }
}
