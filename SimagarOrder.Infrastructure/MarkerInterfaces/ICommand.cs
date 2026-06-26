using MediatR;

namespace SimagarOrder.Infrastructure.MarkerInterfaces;

public interface ICommand<TResponse> : IRequest<TResponse>;
