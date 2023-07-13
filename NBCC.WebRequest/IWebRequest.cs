namespace NBCC.Requests;

public interface IWebRequest
{
    Task PostAsync(string requestUrl);
    Task PostAsync<T>(string requestUrl, T content);
}