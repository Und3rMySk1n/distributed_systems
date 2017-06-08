using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace VowelsService
{
    public class MessageConsumer 
        : IConsumer<Message>
    {
        public async Task Consume(ConsumeContext<Message> context)
        {
            await Console.Out.WriteLineAsync($"Message text: {context.Message.Text}");
        }
    }
}
