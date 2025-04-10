using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace 螢幕擷取工具;

internal static class Program
{
    public static readonly IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]

    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }

}