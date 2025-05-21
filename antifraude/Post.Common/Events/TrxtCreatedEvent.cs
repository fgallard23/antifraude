using Domain.Events;

namespace Post.Common.Events
{
    public class TrxtCreatedEvent : BaseEvent
    {
        public TrxtCreatedEvent() : base(nameof(TrxtCreatedEvent))
        {

        }

        public string sourceAccountId { get; set; }
        public string targetAccountId { get; set; }
        public int transferTypeId { get; set; }
        public double value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
