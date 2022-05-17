using Entities;

namespace Interfaces;

public interface ILogFileConverter
{
    List<Log> ReadFile(string path);
    void CreateOutputFile(List<Log> logs, string path);
}