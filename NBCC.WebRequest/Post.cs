using Microsoft.AspNetCore.Http;
using NBCC.Logging.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NBCC.WebRequest
{
    public class Post: IPost
    {
        IHttpClientFactory HttpClientFactory { get; }
        IAuthenticationSession AuthenticationSession { get; }

        public Post(IHttpClientFactory httpClientFactory, IAuthenticationSession authenticationSession)
        {
            HttpClientFactory = httpClientFactory;
            AuthenticationSession = authenticationSession;
        }

        public async Task PostAsync()
        {
            await PostAsync(string.Empty);
        }

        public async Task PostAsync<T>(T content)
        {
            var json = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            
            using var client = HttpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add(CustomHeaders.AuthenticatedId, 
                AuthenticationSession.AuthenticationId.ToString());
            await client.PostAsync("https://localhost:7283/Instructors", json);
        }
    }

    public interface IPost
    {
        Task PostAsync();
        Task PostAsync<T>(T content);
    }
}