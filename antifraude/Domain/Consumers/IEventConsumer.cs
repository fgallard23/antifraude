namespace Domain.Consumers
{
    public interface IEventConsumer
    {
        Task Consume(string topic);
    }
}
