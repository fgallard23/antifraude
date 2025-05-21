using Confluent.Kafka;
using Domain.Consumers;
using Domain.Producers;
using Microsoft.Extensions.Options;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Converters;
using System.Text.Json;

namespace Post.Cmd.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _config;
        private readonly IEventProducer _eventProducer;
        private readonly IJournalRepository _journalRepository;

        public EventConsumer(IOptions<ConsumerConfig> config, IJournalRepository journalRepository, IEventProducer eventProducer)
        {
            _config = config.Value;
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

                var @event = JsonSerializer.Deserialize<JournalEntity>(consumeResult.Message.Value, options);

                await _journalRepository.UpdateAsync(@event.sourceAccountId, @event.status);

                consumer.Commit(consumeResult);
            }
        }
    }
}
