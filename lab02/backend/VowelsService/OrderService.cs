using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace VowelsService
{  
    public class OrderService
    {
        IBusControl busControl;

        public OrderService()
        {
            busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "submit_order_queue", e =>
                {
                    e.Consumer<SubmitOrderConsumer>();
                });
            });
            busControl.Start();
        }

        ~OrderService()
        {
            busControl.Stop();
        }
    }
}
