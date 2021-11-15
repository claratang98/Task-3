using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using producer.Models;

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

using RabbitMQ.Client;
using System.Text;
using System.Net.Http.Headers;
using System.Threading;

namespace producer.Controllers
{
    [ApiController]
    [Route("")]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        private async Task<HttpResponseMessage> PostTo3D()
        {

            HttpClient externalURL = new HttpClient();
    
            return await externalURL.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
    
        }
        [HttpGet]
        public void Get([FromBody] BpiItem Bpi)
        {  
            //Implement timer for every 15 min
            while (true)
            {
                Thread.Sleep(60 * 15 * 1000);
                Console.WriteLine("*** calling MyMethod *** ");
                var factory = new ConnectionFactory()
                {
                    //HostName = "localhost" , 
                    //Port = 31788
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                    Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBITMQ_PORT"))
                };

                //Console.WriteLine(factory.HostName + ":" + factory.Port);
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Bpi",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                    string message = Bpi.Bpi;
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                        routingKey: "Bpi",
                                        basicProperties: null,
                                        body: body);
                }
            }
        }
    }

    public class BpiItem
    {
        public string Bpi { get; set; }
    }
}