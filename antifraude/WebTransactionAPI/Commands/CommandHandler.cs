using Domain.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace WebTransactionAPI.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

        public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NewJournalCommand command)
        {
            var aggregate = new PostAggregate(
                    command.transferTypeId,
                    command.value
                );

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}
