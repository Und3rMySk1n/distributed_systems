using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Xml.Serialization;
using VowelsServiceLib;
using System.Linq;

namespace VowelsCalculator
{
    class VowelsCalculator
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "analyze",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    object messageAsObject;

                    XmlSerializer serializer = new XmlSerializer(typeof(Data));
                    using (StringReader textReader = new StringReader(message))
                    {
                        messageAsObject = serializer.Deserialize(textReader);
                    }

                    var resultMessage = (Data)messageAsObject;
                    resultMessage.isGood = Analyze(resultMessage.vowels, resultMessage.consonants);

                    Console.WriteLine("Line ID: " + resultMessage.id);
                    Console.WriteLine(resultMessage.value);
                    Console.WriteLine("Is good: " + resultMessage.isGood);
                };
                channel.BasicConsume(queue: "analyze",
                                     noAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Poem analyzer");
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static bool Analyze(int wovels, int consonants)
        {
            double result = wovels / consonants;
            return (result > Math.Floor(4.0 / 6.0) && result < (Math.Floor(4.0 / 6.0) + 1));
        }
    }
}
