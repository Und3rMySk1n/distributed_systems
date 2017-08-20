using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Xml.Serialization;
using VowelsServiceLib;
using System.Linq;

namespace ConsonantsCalculator
{
    class ConsonantsCalculator
    {
        public static void Main()
        {
            MessageProducer _producer = new MessageProducer("analyze");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "consonants",
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
                    resultMessage.consonants = CountConsonants(resultMessage.value);

                    Console.WriteLine("Line ID: " + resultMessage.id);
                    Console.WriteLine("Vowels: " + resultMessage.vowels);
                    Console.WriteLine("Consonants: " + resultMessage.consonants);
                    Console.WriteLine(resultMessage.value);

                    _producer.SendMessage(resultMessage);
                };
                channel.BasicConsume(queue: "consonants",
                                     noAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Consonants calculator");
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static int CountConsonants(string word)
        {
            char[] split = word.ToLowerInvariant().ToCharArray();
            char[] vowels = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z' };
            int count = 0;
            foreach (char vowel in split)
            {
                if (vowels.Contains(vowel))
                    count++;
            }
            return count;
        }
    }
}
