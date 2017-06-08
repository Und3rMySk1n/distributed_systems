using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VowelsService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MassTransit test started...");
            var message = new Message { Text = "This is first message!" };
            var messageService = new MessageService { };
            messageService.Publish(message);
        }
    }
}
