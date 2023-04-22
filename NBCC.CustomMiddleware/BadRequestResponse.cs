using Microsoft.AspNetCore.Http;
using NBCC.WebApplicaion.Extentions;
using System.Net;

namespace NBCC.WebApplicaion;

public static class BadRequestResponse
{
    const string GeneralError = "An unknown error has occurred.  Please contact your systen administrator for further details.";
    public static async Task HandleExceptionAsync(HttpContext httpContext, string errorMessage)
    {
        var response = httpContext.Response;
        response.ContentType = "application/text";
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        
        await response.WriteAsync(errorMessage.NullIfWhiteSpace() ?? GeneralError);
    }
}
