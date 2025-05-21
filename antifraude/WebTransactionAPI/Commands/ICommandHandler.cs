namespace WebTransactionAPI.Commands
{
    public interface ICommandHandler
    {
        Task HandleAsync(NewJournalCommand command);
    }
}
