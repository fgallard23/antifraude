using Domain.Domain;
using Domain.Events;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.Producers;
using Post.Cmd.Domain.Aggregates;
using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;
        private readonly IJournalRepository _journalRepository;

        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer, IJournalRepository journalRepository)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
            _journalRepository = journalRepository;
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (eventStream == null || !eventStream.Any())
                throw new AggregateNotFoundException("Incorrect post ID provided!");

            return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
                throw new ConcurrencyException();

            var version = expectedVersion;

            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(PostAggregate),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                // mongoDB
                await _eventStoreRepository.SaveAsync(eventModel);

                // sqlsever
                var eventData = (TrxtCreatedEvent)eventModel.EventData;
                var journal = new JournalEntity()
                {
                    Id = eventData.Id,
                    sourceAccountId = eventData.sourceAccountId,
                    targetAccountId = eventData.targetAccountId,
                    transferTypeId = eventData.transferTypeId,
                    value = eventData.value,
                    status = "Pending",
                    CreatedAt = DateTime.Now,
                };

                await _journalRepository.CreateAsync(journal);

                // topic
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC_1");
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }
}
