﻿namespace NBCC.CQRS.Commands;

public interface IQueryHandler<in TQuery, TQueryResult>
{
    Task<TQueryResult> Handle(TQuery query);
}
