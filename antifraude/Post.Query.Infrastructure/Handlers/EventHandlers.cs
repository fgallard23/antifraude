using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers
{
    public class EventHandlers : IEventHandlers
    {
        private readonly IJournalRepository _postRepository;

        public EventHandlers(IJournalRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task On(TrxtCreatedEvent @event)
        {
            var post = new JournalEntity
            {
                sourceAccountId = @event.sourceAccountId                
            };

            await _postRepository.CreateAsync(post);
        }
    }
}
