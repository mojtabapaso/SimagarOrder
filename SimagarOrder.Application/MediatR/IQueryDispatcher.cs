using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.MediatR;

public interface IQueryDispatcher : IScopedDependency
{
    Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query);
}
