using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.MediatR;

public interface ICommandDispatcher : IScopedDependency
{
    Task<TResponse> Dispatch<TResponse>(ICommand<TResponse> command);
}
