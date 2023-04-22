using Microsoft.Extensions.DependencyInjection;

namespace NBCC.Courses.Commands;

public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query)
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return await handler.Handle(query);
    }
}
