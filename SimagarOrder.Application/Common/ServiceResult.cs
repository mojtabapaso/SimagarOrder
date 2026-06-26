using Microsoft.AspNetCore.Http;

namespace SimagarOrder.Application.Common;

public class ServiceResult
{
    public bool IsSuccess { get; protected init; }

    public string? Message { get; protected init; }

    public int StatusCode { get; protected init; }

    public static ServiceResult Success(string? message = null)
        => new()
        {
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = message
        };

    public static ServiceResult Failure(
        string message,
        int statusCode = StatusCodes.Status400BadRequest)
        => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message
        };

    public static ServiceResult NotFound(
    string message,
    int statusCode = StatusCodes.Status404NotFound)
    => new()
    {
        IsSuccess = false,
        StatusCode = statusCode,
        Message = message
    };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; private init; }

    public static ServiceResult<T> Success(
        T data,
        string? message = null)
        => new()
        {
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = message,
            Data = data
        };

    public static new ServiceResult<T> Failure(
        string message,
        int statusCode = StatusCodes.Status400BadRequest)
        => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message
        };
    public static new ServiceResult<T> NotFound(
    string message,
    int statusCode = StatusCodes.Status404NotFound)
    => new()
    {
        IsSuccess = false,
        StatusCode = statusCode,
        Message = message
    };
}