namespace SimagarOrder.Domain.Entities;


public class DomainException : Exception
{
    public override string Message { get; }

    public DomainException(string Message)
    {
        this.Message = Message;
    }
}

