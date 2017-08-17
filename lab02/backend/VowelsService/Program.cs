using System;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using RabbitMQ.Client;
using System.Text;

namespace VowelsService
{
    public class Program
    {
        static void Main(string[] args)
        {
            

            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/values").Result;

                Console.WriteLine(response);
                Console.WriteLine("Self-hosted poem analyzer");
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
        }
    }
}
