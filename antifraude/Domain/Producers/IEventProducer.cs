using Domain.Events;
using Post.Query.Domain.Entities;

namespace Domain.Producers
{
    public interface IEventProducer
    {
        Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
        Task ProduceAsync(string topic, JournalEntity entity) ;
    }
}
