using System.Text;
using System.Text.RegularExpressions;

namespace LogFileConverter.Entities;

public partial class LogFile
{
    private static string Version { get; set; } = "1.0.";
    private static DateTime Date { get; set; } = DateTime.Now;
    
    public string Provider { get; private set; }
    public string HttpMethod { get; private set; }
    public int StatusCode { get; private set; }
    public string UriPath { get; private set; }
    public int TimeTaken { get; private set; }
    public int ResponseSize { get; private set; }
    public string CacheStatus { get; private set; }

    public static List<LogFile> ReadFile(string path)
    {
        var logList = new List<LogFile>();
        var uriRegEx = new Regex("https?", RegexOptions.Compiled);
        
        using (var fs = uriRegEx.IsMatch(path)
                           ? GetFromUri(path)
                           : new FileStream(path, FileMode.Open))
        using (var reader = new StreamReader(fs))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var log = ConvertStringToLogFile(line);
                logList.Add(log);
            }
        }
        
        return logList;
    }

    public static void CreateOutputFile(List<LogFile>logs, string path)
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
                    , log.Provider
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

    private static LogFile ConvertStringToLogFile(string line)
    {
        string[] fields = line.Split('|');
        string httpMethod = fields[3].Split(' ')[0];
        string uriPath = fields[3].Split(' ')[1];
        decimal responseSize = Convert.ToDecimal(fields[4]);
        var log = new LogFile();
        
        log.Provider = "MINHA CDN";
        log.ResponseSize = Convert.ToInt32(fields[0]);
        log.StatusCode = Convert.ToInt32(fields[1]);
        log.CacheStatus = fields[2];
        log.HttpMethod = httpMethod;
        log.UriPath = uriPath;
        log.ResponseSize = Convert.ToInt32(responseSize);
        
        return log;
    }
}