using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace NBCC.WebApplicaion;

public class CustomMiddleware
{
    RequestDelegate Next { get; }

    public CustomMiddleware(RequestDelegate next) => Next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await Next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        string result = ex.Message;

        if (ex is SqlException xx)
        {
            StringBuilder errorMessages = new();

            for (int i = 0; i < xx.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                    "Message: " + xx.Errors[i].Message + "\n" +
                    "Error Number: " + xx.Errors[i].Number + "\n" +
                    "LineNumber: " + xx.Errors[i].LineNumber + "\n" +
                    "Source: " + xx.Errors[i].Source + "\n" +
                    "Procedure: " + xx.Errors[i].Procedure + "\n");
            }
            result = errorMessages.ToString();
        }
        var response = httpContext.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        await response.WriteAsync(result);
    }
}
