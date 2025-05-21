﻿using Domain.Infrastructure;
using Domain.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher<JournalEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<JournalEntity>>>> _handlers = new();

        public void RegisterHandlers<TQuery>(Func<TQuery, Task<List<JournalEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException("You cannot register the same query handler twice!");
            }
            _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
        }

        public async Task<List<JournalEntity>> SendAsync(BaseQuery query)
        {
            if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<JournalEntity>>> handler))
            {
                return await handler(query);
            }

            throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
        }
    }
}
