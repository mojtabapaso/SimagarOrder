using Microsoft.AspNetCore.Http;

namespace SimagarOrder.Application.Common;

/// <summary>
/// کلاس پایه برای بازگرداندن نتیجه عملیات سرویس‌ها
/// </summary>
public class ServiceResult
{
    /// <summary>
    /// مشخص می‌کند عملیات با موفقیت انجام شده است یا خیر.
    /// </summary>
    public bool IsSuccess { get; protected init; }

    /// <summary>
    /// پیام مربوط به نتیجه عملیات
    /// </summary>
    public string? Message { get; protected init; }

    /// <summary>
    /// کد وضعیت HTTP متناظر با نتیجه عملیات
    /// </summary>
    public int StatusCode { get; protected init; }

    /// <summary>
    /// ایجاد نتیجه موفق
    /// </summary>
    public static ServiceResult Success(string? message = null)
        => new()
        {
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = message
        };

    /// <summary>
    /// ایجاد نتیجه ناموفق
    /// </summary>
    public static ServiceResult Failure(
        string message,
        int statusCode = StatusCodes.Status400BadRequest)
        => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message
        };

    /// <summary>
    /// ایجاد نتیجه در صورت پیدا نشدن داده
    /// </summary>
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

/// <summary>
/// نتیجه عملیات به همراه داده خروجی
/// </summary>
/// <typeparam name="T">نوع داده خروجی</typeparam>
public class ServiceResult<T> : ServiceResult
{
    /// <summary>
    /// داده بازگشتی عملیات
    /// </summary>
    public T? Data { get; private init; }

    /// <summary>
    /// ایجاد نتیجه موفق به همراه داده
    /// </summary>
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

    /// <summary>
    /// ایجاد نتیجه ناموفق
    /// </summary>
    public static new ServiceResult<T> Failure(
        string message,
        int statusCode = StatusCodes.Status400BadRequest)
        => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message
        };

    /// <summary>
    /// ایجاد نتیجه در صورت پیدا نشدن داده
    /// </summary>
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