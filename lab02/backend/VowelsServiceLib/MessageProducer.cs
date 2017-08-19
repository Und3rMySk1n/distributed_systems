using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Xml.Serialization;
using VowelsServiceLib;

namespace VowelsServiceLib
{
    public class MessageProducer
    {
        private string _queueName;
        private IConnection _connection;
        private IModel _channel;

        public MessageProducer(string queueName)
        {
            _queueName = queueName;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
        }

        ~MessageProducer()
        {
            _channel.Close(200, "Goodbye");
            _connection.Close();
        }

        public void SendMessage(Data message)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Data));
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, message);
                var body = Encoding.UTF8.GetBytes(textWriter.ToString());

                _channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
