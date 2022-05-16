namespace LogFileConverter.Entities;

public struct DirectoryFunctions
{
    public static void CreateDirectoryIfNotExists(string path)
    {
        var pathWithoutFileName = string.Join('/',
            path.Split('/').Where(c => !c.EndsWith(".txt")).ToList()
        );
        if (!Directory.Exists(pathWithoutFileName))
        {
            Directory.CreateDirectory(pathWithoutFileName);
        }
    }
}