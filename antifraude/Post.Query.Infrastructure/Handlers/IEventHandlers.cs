using Post.Common.Events;

namespace Post.Query.Infrastructure.Handlers
{
    public interface IEventHandlers
    {
        Task On(TrxtCreatedEvent @event);
    }
}
