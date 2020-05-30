using System;
using System.Text;
using RabbitMQ.Client;

namespace Ex4Publisher
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "docker.local" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "email", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "whatsapp", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "external", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.ExchangeDeclare(exchange: "alerts", type: "topic");

                channel.QueueBind(queue: "email", exchange: "alerts", routingKey: "*.web.*");
                channel.QueueBind(queue: "whatsapp", exchange: "alerts", routingKey: "*.mobile.*");
                channel.QueueBind(queue: "external", exchange: "alerts", routingKey: "game.#");

                var count = 0;

                while (true)
                {
                    string message = $"Alert message! {count++}";

                    var body1 = Encoding.UTF8.GetBytes($"{message} - payments");
                    var body2 = Encoding.UTF8.GetBytes($"{message} - orders");
                    var body3 = Encoding.UTF8.GetBytes($"{message} - clients");
                    var body4 = Encoding.UTF8.GetBytes($"{message} - game");

                    channel.BasicPublish(exchange: "alerts", routingKey: "payments.web.exec", basicProperties: null, body: body1);
                    channel.BasicPublish(exchange: "alerts", routingKey: "orders.web.exec", basicProperties: null, body: body2);
                    channel.BasicPublish(exchange: "alerts", routingKey: "clients.web.exec", basicProperties: null, body: body3);

                    channel.BasicPublish(exchange: "alerts", routingKey: "game.mobile.playing", basicProperties: null, body: body4);

                    Console.WriteLine($"[x] Sent: {message}");

                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
