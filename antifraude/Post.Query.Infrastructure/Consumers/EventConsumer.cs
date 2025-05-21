using Confluent.Kafka;
using Domain.Consumers;
using Domain.Events;
using Domain.Producers;
using Microsoft.Extensions.Options;
using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;
using System.Text.Json;

namespace Post.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _config;
        private readonly IEventProducer _eventProducer;
        private readonly IEventHandlers _eventHandler;
        private readonly IJournalRepository _journalRepository;

        public EventConsumer(IOptions<ConsumerConfig> config, IEventHandlers eventHandler, IJournalRepository journalRepository, IEventProducer eventProducer)
        {
            _config = config.Value;
            _eventHandler = eventHandler;
            _journalRepository = journalRepository;
            _eventProducer = eventProducer; 
        }

        public async Task Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_config)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            consumer.Subscribe(topic);

            var newTopic = Environment.GetEnvironmentVariable("KAFKA_TOPIC_2");

            while (true)
            {
                var consumeResult = consumer.Consume();

                if (consumeResult?.Message == null) continue;

                var options = new JsonSerializerOptions
                {
                    Converters = { new EventJsonConverter() }
                };

                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
                var journal = (TrxtCreatedEvent) @event;

                // new event
                var newJournalEvent = new JournalEntity()
                {
                    sourceAccountId = journal.sourceAccountId
                };

                var amount = _journalRepository.TrxAmountAsync(journal.sourceAccountId);
                if (journal?.value > 2000) // amount > 2000
                {
                    newJournalEvent.status = "Rejected";
                }
                else if (amount > 20000) // sum by day > 20000
                {
                    newJournalEvent.status = "Rejected";
                }
                else 
                {
                    newJournalEvent.status = "Approved";
                }

                // add new topic
                await _eventProducer.ProduceAsync(newTopic, newJournalEvent);

                consumer.Commit(consumeResult);
            }
        }
    }
}
