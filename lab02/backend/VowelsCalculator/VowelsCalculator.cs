using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Xml.Serialization;
using VowelsServiceLib;
using System.Linq;
using System.Diagnostics;

namespace VowelsCalculator
{
    class VowelsCalculator
    {
        public static void Main()
        {
            MessageProducer _producer = new MessageProducer("consonants");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "vowels",
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
                    resultMessage.vowels = CountVowels(resultMessage.value);

                    Debug.WriteLine("Line ID: " + resultMessage.id);
                    Debug.WriteLine("Vowels: " + resultMessage.vowels);
                    Debug.WriteLine("Consonants: " + resultMessage.consonants);
                    Debug.WriteLine(resultMessage.value);

                    _producer.SendMessage(resultMessage);
                };
                channel.BasicConsume(queue: "vowels",
                                     noAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Vowels calculator");
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
