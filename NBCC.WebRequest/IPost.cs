namespace NBCC.WebRequest;

public interface IPost
{
    Task PostAsync(string requestUrl);
    Task PostAsync<T>(string requestUrl, T content);
}