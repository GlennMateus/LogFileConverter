namespace LogFileConverter.Entities;

public partial class LogFile
{
    public static Stream GetFromUri(string uri)
    {
        var request = new HttpClient()
            .GetAsync(uri)
            .Result;
        return request.Content.ReadAsStream();
    }
}