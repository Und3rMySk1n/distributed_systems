using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Xml.Serialization;
using VowelsServiceLib;
using System.Linq;
using System.Configuration;
using System.Diagnostics;

namespace PoemAnalyzer
{
    class PoemAnalyzer
    {
        private string _resultPoem = "";
        private int _stringsNumber = 0;
        IStorage _storage = new RedisStorage();

        public static void Main()
        {
            PoemAnalyzer analyzer = new PoemAnalyzer();            

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

                    Debug.WriteLine("Line ID: " + resultMessage.id);
                    Debug.WriteLine("Is good: " + resultMessage.isGood);
                    Debug.WriteLine(resultMessage.value);

                    analyzer.PrepareStorage();
                    analyzer.PreparePoem(resultMessage);
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
            return (wovels == (Math.Floor(consonants * 4.0 / 6.0)));
        }

        private void PrepareStorage()
        {
            if (_stringsNumber == 0)
            {
                _storage.Delete("0");
            }
        }

        private void PreparePoem(Data poemString)
        {
            if (poemString.isGood)
            {
                _resultPoem += poemString.value;
            }

            _stringsNumber++;
            if (_stringsNumber == poemString.count)
            {
                SavePoem();
            }
        }

        private void SavePoem()
        {
            _storage.Save("0", _resultPoem);
            ResetData();
        }

        private void ResetData()
        {
            _stringsNumber = 0;
            _resultPoem = "";
        }
    }
}
