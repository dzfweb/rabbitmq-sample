using MassTransit;
using RabbitMQ.Sample.Common;
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
                var host = cfg.Host(new Uri(RabbitMQConstants.RabbitMqUri), h =>
                {
                    h.Username(RabbitMQConstants.RabbitMqUser);
                    h.Password(RabbitMQConstants.RabbitMqPassword);
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