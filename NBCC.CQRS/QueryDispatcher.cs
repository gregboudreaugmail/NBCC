﻿namespace NBCC.CQRS.Commands;

public sealed class QueryDispatcher : IQueryDispatcher
{
    IServiceProvider ServiceProvider { get; }

    public QueryDispatcher(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query)
    {
        var handler = ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return await handler.Handle(query);
    }
}
