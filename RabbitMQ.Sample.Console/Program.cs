using MassTransit;
using RabbitMQ.Sample.Console.Consumers;
using System;

namespace RabbitMQ.Sample.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "AddUser1", e =>
                {
                    e.Consumer<AddUserConsumer>();
                });
            });
            busControl.Start();
        }
    }
}