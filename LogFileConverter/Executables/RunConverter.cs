public static partial class Program
{
    public static void Run()
    {
        Console.WriteLine("Informe os diretórios de Input e Output, separados por um espaço em branco");
        string[] args = Console.ReadLine().Split(' ');
        string inputFilePath = args[0];
        string outputPath = args[1];

        var logList = _logFileConverter.ReadFile(inputFilePath);
        _logFileConverter.CreateOutputFile(logList, outputPath);
    }
}