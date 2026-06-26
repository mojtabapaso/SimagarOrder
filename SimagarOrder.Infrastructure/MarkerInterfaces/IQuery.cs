using MediatR;

namespace SimagarOrder.Infrastructure.MarkerInterfaces;

public interface IQuery<TResponse> : IRequest<TResponse>;
