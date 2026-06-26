using MediatR;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.MediatR;

public class QueryDispatcher(IMediator mediator) : IQueryDispatcher
{
    public Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query)
    {
        return mediator.Send(query);
    }
}
