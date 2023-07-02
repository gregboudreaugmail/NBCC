using Microsoft.AspNetCore.Http;
using NBCC.Logging.Models;
using Newtonsoft.Json;
using System.Text;

namespace NBCC.WebRequest;

public class Post : IPost
{
    IHttpClientFactory HttpClientFactory { get; }
    IAuthenticationSession AuthenticationSession { get; }
    IHttpContextAccessor HttpContextAccessorAccessor { get; }

    public Post(IHttpClientFactory httpClientFactory, IAuthenticationSession authenticationSession, IHttpContextAccessor httpContextAccessorAccessor)
    {
        HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        AuthenticationSession = authenticationSession ?? throw new ArgumentNullException(nameof(authenticationSession));
        HttpContextAccessorAccessor = httpContextAccessorAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessorAccessor));
    }

    public async Task PostAsync(string requestUrl)
    {
        await PostAsync(requestUrl, string.Empty);
    }

    public async Task PostAsync<T>(string requestUrl, T content)
    {
        var urlParameters = IsSimple(typeof(T)) ? JsonConvert.SerializeObject(content) : string.Empty;
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{requestUrl}{urlParameters}")
        {
            Headers =
            {
                { "Authorization", HttpContextAccessorAccessor.HttpContext.Request.Headers["Authorization"].ToString() },
                { CustomHeaders.AuthenticatedId, AuthenticationSession.AuthenticationId.ToString() }
            }
        };

        if (!IsSimple(typeof(T)))
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        var response = await HttpClientFactory.CreateClient().SendAsync(requestMessage);

    }

    static bool IsSimple(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            return IsSimple(type.GetGenericArguments()[0]);

        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal);
    }
}