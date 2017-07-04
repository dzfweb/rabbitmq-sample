namespace RabbitMQ.Sample.Common.Messages.Commands
{
    public interface IAddUserCommand
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}