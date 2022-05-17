using Entities;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static partial class Program
{
    private static ILogFileConverter _logFileConverter { get; set; }
    public static void Main()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _logFileConverter = serviceProvider.GetService<ILogFileConverter>() 
                            ?? throw new InvalidOperationException();
        Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ILogFileConverter, LogConverter>();
    }

    
}