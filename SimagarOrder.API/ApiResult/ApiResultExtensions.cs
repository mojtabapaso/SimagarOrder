using Microsoft.AspNetCore.Mvc;
using SimagarOrder.Application.Common;

namespace SimagarOrder.API.ApiResult;

public static class ApiResultExtensions
{
    public static IActionResult ToApiResult(this ServiceResult result)
    {
        return new ObjectResult(result)
        {
            StatusCode = result.StatusCode
        };
    }

}