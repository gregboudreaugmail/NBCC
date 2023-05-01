using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace NBCC.Logging.Models;

public sealed class AuthenticationSessionSession : IAuthenticationSession
{
    IHttpContextAccessor HttpContextAccessor { get; }

    public AuthenticationSessionSession(IHttpContextAccessor httpContextAccessor) => HttpContextAccessor = httpContextAccessor;
    public int UserId {
        get
        {
            HttpContextAccessor.HttpContext.Session
                .TryGetValue(CachedItems.UserId, out var authenticationId);
            return Deserialize<int>(authenticationId);
        }
    }

    public int AuthenticationId
    {
        get
        {
            HttpContextAccessor.HttpContext.Session
                .TryGetValue(CachedItems.AuthenticatedId, out var authenticationId);
            return Deserialize<int>(authenticationId);
        }
    }

    public void AssignAuthentication(int authenticationId, int userId)
    {
        HttpContextAccessor.HttpContext.Session.Set(CachedItems.AuthenticatedId, ObjectToBytes(authenticationId));
        HttpContextAccessor.HttpContext.Session.Set(CachedItems.UserId, ObjectToBytes(userId));
    }

    static T Deserialize<T>(byte[] param)
    {
        var result = Encoding.UTF8.GetString(param);
        return JsonConvert.DeserializeObject<T>(result);
    }

    static byte[] ObjectToBytes(object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var serializedResult = Encoding.UTF8.GetBytes(json);
        return serializedResult;
    }
}