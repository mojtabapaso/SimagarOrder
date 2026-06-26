using MediatR;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.MediatR;

public class CommandDispatcher(IMediator mediator) : ICommandDispatcher
{
    public Task<TResponse> Dispatch<TResponse>(ICommand<TResponse> command)
    {
        return mediator.Send(command);
    }
}