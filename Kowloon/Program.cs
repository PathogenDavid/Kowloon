using Kowloon.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Kowloon
{
    public static class Program
    {
        public static KowloonManager Kowloon { get; private set; }

        public static void Main(string[] args)
        {
            using (Kowloon = new KowloonManager())
            {
                CreateHostBuilder(args).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:80");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
