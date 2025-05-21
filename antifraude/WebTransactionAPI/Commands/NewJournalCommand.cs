using Domain.Commands;

namespace WebTransactionAPI.Commands
{
    public class NewJournalCommand : BaseCommand
    {
        public int transferTypeId { get; set; }
        public double value { get; set; }
    }
}
