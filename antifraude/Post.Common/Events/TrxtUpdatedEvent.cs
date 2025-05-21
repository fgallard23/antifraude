using Domain.Events;

namespace Post.Common.Events
{
    public class TrxtUpdatedEvent : BaseEvent
    {
        public TrxtUpdatedEvent() : base(nameof(TrxtUpdatedEvent))
        {
        }

        public Guid sourceAccountId { get; set; }
        public Guid targetAccountId { get; set; }
        public int transferTypeId { get; set; }
        public double value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
