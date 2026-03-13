namespace LXP.Api.Interceptors;

using System.Net;
using System.Reflection;
using LXP.Common.Constants;
using LXP.Common.Enums;
using LXP.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// API Exception Interceptor
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="logger"></param>
public class ApiExceptionInterceptor(ILogger<ApiExceptionInterceptor> logger)
    : IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionInterceptor> _logger = logger;

    /// <summary>
    /// On Exception Implementation
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var statusCode =
            (
                (context.Exception is WebException)
                && ((HttpWebResponse)(context.Exception as WebException).Response) != null
            )
                ? ((HttpWebResponse)(context.Exception as WebException).Response).StatusCode
                : GetErrorCode(context.Exception.GetType());

        var exception = context.Exception;
        context.HttpContext.Response.StatusCode = (int)statusCode;

        //Business exception-More generics for external world
        var error = new APIResponse()
        {
            StatusCode = (int)statusCode,
            Message =
                (int)statusCode == (int)HttpStatusCode.InternalServerError
                    ? MessageConstants.MsgServerError
                    : exception.Message ?? exception.InnerException.Message
        };

        //Logs your technical exception with stack trace below
        this._logger.LogError(
            "{FullName} Exception: {Message} Inner Exception: {InnerExceptionMessage} Stack Trace: {StackTrace}",
            MethodBase.GetCurrentMethod().DeclaringType.FullName,
            exception.Message ?? "None",
            exception.InnerException?.Message ?? "None",
            exception.StackTrace ?? "None"
        );

        context.Result = new JsonResult(error);
        return Task.CompletedTask;
    }

    private static HttpStatusCode GetErrorCode(Type exceptionType)
    {
        if (Enum.TryParse(exceptionType.Name, out Exceptions tryParseResult))
        {
            return tryParseResult switch
            {
                Exceptions.UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                Exceptions.ArgumentException => HttpStatusCode.BadRequest,
                Exceptions.ArgumentNullException => HttpStatusCode.BadRequest,
                Exceptions.ArgumentOutOfRangeException => HttpStatusCode.BadRequest,
                Exceptions.FormatException => HttpStatusCode.BadRequest,
                Exceptions.InvalidEnumArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError,
            };
        }
        else
        {
            return HttpStatusCode.InternalServerError;
        }
    }
}
