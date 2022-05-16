using LogFileConverter.Entities;

public static class Program
{
    public static void Main()
    {
        string[] args = Console.ReadLine().Split(' ');
        string inputFilePath = args[0];
        string outputPath = args[1];

        var logList = LogFile.ReadFile(inputFilePath);
        LogFile.CreateOutputFile(logList, outputPath);
    }
}