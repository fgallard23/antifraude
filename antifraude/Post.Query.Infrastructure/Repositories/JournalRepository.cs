using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class JournalRepository : IJournalRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public JournalRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(JournalEntity journal)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.JournalEntity.Add(journal);

            _ = await context.SaveChangesAsync();
        }

        public double TrxAmountAsync(string sourceAccountId)
        {
            var dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            using DatabaseContext context = _contextFactory.CreateDbContext();
            return context.JournalEntity
                   .Where(x => x.sourceAccountId.Contains(sourceAccountId) && x.CreatedAt > dateTime)
                   .Select(x => x.value).Sum();
        }

        public async Task UpdateAsync(string sourceAccountId, string status)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var journal = context.JournalEntity.Where(x => x.sourceAccountId.Contains(sourceAccountId)).FirstOrDefault();
            if (journal != null) 
            { 
                journal.status = status;
                context.JournalEntity.Update(journal);
            }

            _ = await context.SaveChangesAsync();
        }
    }
}
