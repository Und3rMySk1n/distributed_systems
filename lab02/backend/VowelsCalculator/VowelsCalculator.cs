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
                channel.QueueDeclare(queue: "hello",
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
                    resultMessage.vowels = CountVowels(resultMessage.value).ToString();

                    Console.WriteLine("Vowels: " + resultMessage.vowels);
                    Console.WriteLine("Line number: " + resultMessage.id);
                    Console.WriteLine(resultMessage.value);
                };
                channel.BasicConsume(queue: "hello",
                                     noAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static int CountVowels(string word)
        {
            char[] split = word.ToLowerInvariant().ToCharArray();
            char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };
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
