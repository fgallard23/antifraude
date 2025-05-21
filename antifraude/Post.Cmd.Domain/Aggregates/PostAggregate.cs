using Domain.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        public PostAggregate()
        {

        }
        public PostAggregate(int transferTypeId, double value)
        {
            RaiseEvent(new TrxtCreatedEvent
            {
                sourceAccountId = Guid.NewGuid().ToString(),
                targetAccountId = Guid.NewGuid().ToString(),
                transferTypeId = transferTypeId,
                value = value,
            });
        }

        public void Apply(TrxtCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
        }
    }
}
