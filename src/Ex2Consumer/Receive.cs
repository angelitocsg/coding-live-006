using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ex2Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "docker.local" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName1 = "myQueue1";
                var queueName2 = "myQueue2";

                channel.QueueDeclare(queue: queueName1, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: queueName2, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer1 = new EventingBasicConsumer(channel);
                var consumer2 = new EventingBasicConsumer(channel);

                consumer1.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"[x] C1 - Received: {message}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    System.Threading.Thread.Sleep(50);
                };

                consumer2.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"[x] C2 - Received: {message}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    System.Threading.Thread.Sleep(50);
                };

                channel.BasicConsume(queue: queueName1, autoAck: false, consumer: consumer1);
                channel.BasicConsume(queue: queueName2, autoAck: false, consumer: consumer2);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}