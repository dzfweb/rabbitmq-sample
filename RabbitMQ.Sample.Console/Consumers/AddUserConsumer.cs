using MassTransit;
using RabbitMQ.Sample.Common.Messages.Commands;
using System.Threading.Tasks;

namespace RabbitMQ.Sample.Console.Consumers
{
    public class AddUserConsumer : IConsumer<IAddUserCommand>
    {
        public Task Consume(ConsumeContext<IAddUserCommand> context)
        {
            System.Console.WriteLine($"Adding user {context.Message.FirstName} {context.Message.LastName}");
            return Task.CompletedTask;
        }
    }
}