using Microsoft.AspNetCore.Http;
using NBCC.WebApplication.Extensions;
using System.Net;

namespace NBCC.WebApplication;

public static class BadRequestResponse
{
    const string GeneralError = "An unknown error has occurred.  Please contact your systen administrator for further details.";
    public static async Task HandleExceptionAsync(HttpContext httpContext,
        string errorMessage, int? messageId)
    {
        var response = httpContext.Response;
        response.ContentType = "application/text";
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        var referenceMessage = messageId != null ? $"Reference Number: {messageId}" : string.Empty;
        await response.WriteAsync($"{errorMessage.NullIfWhiteSpace() ?? GeneralError} {referenceMessage}");
    }
}
