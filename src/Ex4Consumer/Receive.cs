using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ex4Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "docker.local" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.BasicQos(0, prefetchCount: 3, true);

                channel.QueueDeclare(queue: "email", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "whatsapp", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "external", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer1 = new EventingBasicConsumer(channel);
                var consumer2 = new EventingBasicConsumer(channel);
                var consumer3 = new EventingBasicConsumer(channel);

                consumer1.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"[x] Send Email: {message}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    System.Threading.Thread.Sleep(200);
                };

                consumer2.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"[x] Send WhatsApp: {message}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    System.Threading.Thread.Sleep(200);
                };

                consumer3.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"[x] Send External: {message}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    System.Threading.Thread.Sleep(200);
                };

                channel.BasicConsume(queue: "email", autoAck: false, consumer: consumer1);
                channel.BasicConsume(queue: "whatsapp", autoAck: false, consumer: consumer2);
                channel.BasicConsume(queue: "external", autoAck: false, consumer: consumer3);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}