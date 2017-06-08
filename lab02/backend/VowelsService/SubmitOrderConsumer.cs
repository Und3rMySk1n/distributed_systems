using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace VowelsService
{
    public class SubmitOrderConsumer 
        : IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            await Console.Out.WriteLineAsync($"Submit Date: {context.Message.SubmitDate}");
            await Console.Out.WriteLineAsync($"Customer Number: {context.Message.CustomerNumber}");
            await Console.Out.WriteLineAsync($"Order Number: {context.Message.OrderNumber}");
        }
    }
}
