using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface IJournalRepository
    {
        Task CreateAsync(JournalEntity journal);
        Task UpdateAsync(string sourceAccountId, string status);
        double TrxAmountAsync(string sourceAccountId);
    }
}
