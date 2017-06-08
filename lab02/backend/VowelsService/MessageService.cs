using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace VowelsService
{  
    public class MessageService
    {
        IBusControl busControl;

        public MessageService()
        {
            busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "message_queue", e =>
                {
                    e.Consumer<MessageConsumer>();
                });
            });
            busControl.Start();
        }

        public void Publish(Message message)
        {
            busControl.Publish<Message>(message);
        }

        ~MessageService()
        {
            busControl.Stop();
        }
    }
}
