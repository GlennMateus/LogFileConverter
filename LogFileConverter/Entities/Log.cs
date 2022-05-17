namespace Entities;

public class Log
{
    public Log(string provider, string httpMethod, int statusCode, string uriPath, int timeTaken, int responseSize, string cacheStatus)
    {
        Provider = provider;
        HttpMethod = httpMethod;
        StatusCode = statusCode;
        UriPath = uriPath;
        TimeTaken = timeTaken;
        ResponseSize = responseSize;
        CacheStatus = cacheStatus.ToUpper().Equals("INVALIDATE")
            ? "REFRESH_HIT"
            : cacheStatus;
    }
    public string Provider { get; }
    public string HttpMethod { get; }
    public int StatusCode { get; }
    public string UriPath { get; }
    public int TimeTaken { get; }
    public int ResponseSize { get; }
    public string CacheStatus { get; }
}