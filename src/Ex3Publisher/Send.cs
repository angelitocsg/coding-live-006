using System;
using System.Text;
using RabbitMQ.Client;

namespace Ex3Publisher
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "docker.local" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "imageProcess", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "imageArchive", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.ExchangeDeclare(exchange: "image", type: "direct");

                channel.QueueBind(queue: "imageProcess", exchange: "image", routingKey: "crop");
                channel.QueueBind(queue: "imageProcess", exchange: "image", routingKey: "resize");
                channel.QueueBind(queue: "imageArchive", exchange: "image", routingKey: "resize");

                var count = 0;

                while (true)
                {
                    string message = $"Image process message! {count++}";

                    var body1 = Encoding.UTF8.GetBytes($"{message} - crop");
                    var body2 = Encoding.UTF8.GetBytes($"{message} - resize");

                    channel.BasicPublish(exchange: "image", routingKey: "crop", basicProperties: null, body: body1);
                    channel.BasicPublish(exchange: "image", routingKey: "resize", basicProperties: null, body: body2);

                    Console.WriteLine($"[x] Sent: {message}");

                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
