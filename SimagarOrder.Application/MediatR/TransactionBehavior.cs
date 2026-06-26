using MediatR;
using SimagarOrder.Infrastructure.MarkerInterfaces;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.MediatR;

public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork):IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICommand<TResponse>)
            return await next();
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            var response = await next();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}