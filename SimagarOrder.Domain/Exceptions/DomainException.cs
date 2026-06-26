namespace SimagarOrder.Domain.Exceptions;

/// <summary>
/// استثنای مربوط به قوانین دامنه (Business Rules).
/// زمانی پرتاب می‌شود که یکی از قوانین Domain نقض شود.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// پیام مربوط به خطای دامنه
    /// </summary>
    public DomainException(string message) : base(message)
    {
    }
}