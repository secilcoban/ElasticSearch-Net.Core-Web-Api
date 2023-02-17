using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;

namespace ElasticSearchApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        //public static IHostBuilder CreateHostBuilder2(string[] args) =>
        //  Host.CreateDefaultBuilder(args)
        //      .ConfigureWebHostDefaults(webBuilder =>
        //      {
        //          var exePath = Process.GetCurrentProcess().MainModule.FileName;
        //          var rootFolder = Path.GetDirectoryName(exePath);
        //          webBuilder.UseContentRoot(rootFolder)
        //                        .UseStartup<Startup>()
        //                        .UseUrls("http://localhost:3403")
        //                        .Build().Run();
        //      });
    }
}
