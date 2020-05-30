using System;
using System.Text;
using RabbitMQ.Client;

namespace Ex2Publisher
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "docker.local" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "myQueue1", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "myQueue2", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                channel.QueueBind(queue: "myQueue1", exchange: "logs", "");
                channel.QueueBind(queue: "myQueue2", exchange: "logs", "");

                var count = 0;

                while (true)
                {
                    string message = $"Fanout message! {count++}";

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);

                    Console.WriteLine($"[x] Sent: {message}");

                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
