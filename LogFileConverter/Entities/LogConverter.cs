using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Interfaces;

namespace Entities;

public class LogConverter : ILogFileConverter
{
    private static string Version { get; set; }
    private static DateTime Date { get; set; }

    public LogConverter()
    {
        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0";
        Date = DateTime.Now;
    }
    
    public List<Log> ReadFile(string path)
    {
        var logList = new List<Log>();
        var uriRegEx = new Regex("https?", RegexOptions.Compiled);
        
        using (var fs = uriRegEx.IsMatch(path)
                           ? GetFromUri(path)
                           : new FileStream(path, FileMode.Open))
        using (var reader = new StreamReader(fs))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var log = ConvertStringToEntityLog(line);
                logList.Add(log);
            }
        }
        return logList;
    }
    
    public void CreateOutputFile(List<Log>logs, string path)
    {
        DirectoryFunctions.CreateDirectoryIfNotExists(path);
        var s = new StringBuilder();
        s.AppendLine($"#Version: {Version}");
        s.AppendLine($"#Date: {Date:dd/MM/yyyy HH:mm:ss}");
        s.AppendLine("#Fields: provider http-method status-code uri-path time-taken response-size cache-status");
        
        foreach (var log in logs)
        {
            s.AppendLine(
                string.Join(
                    "\t"
                    , $"\"{log.Provider}\""
                    , log.HttpMethod
                    , log.StatusCode.ToString()
                    , log.UriPath
                    , log.TimeTaken.ToString()
                    , log.ResponseSize.ToString()
                    , log.CacheStatus
                )
            );
        }
        File.WriteAllText(path, s.ToString());
    }

    private Log ConvertStringToEntityLog(string line)
    {
        /* Original
         *  0   1   2     3                         4
         * 199|404|MISS|"GET /not-found HTTP/1.1"|142.9
         * New
         * "MINHA CDN" GET 404 /not-found 143 199 MISS
         * provider http-method status-code uri-path time-taken response-size cache-status
         */
        string[] fields = line.Split('|');
        var provider = "MINHA CDN";
        var responseSize = Convert.ToInt32(fields[0]);
        var statusCode = Convert.ToInt32(fields[1]);
        var cacheStatus = fields[2];
        var httpMethod = fields[3].Split(' ')[0].Replace("\"", "");
        var uriPath = fields[3].Split(' ')[1].Replace("\"", "");
        var timeTaken = Convert.ToInt32(Convert.ToDecimal(fields[4]));
        
        var log = new Log(
            provider,
            httpMethod,
            statusCode,
            uriPath,
            timeTaken,
            responseSize,
            cacheStatus
        );

        return log;
    }
    
    private Stream GetFromUri(string uri)
    {
        var request = new HttpClient()
            .GetAsync(uri)
            .Result;
        return request.Content.ReadAsStream();
    }
}