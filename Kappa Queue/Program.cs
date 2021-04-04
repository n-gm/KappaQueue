using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace KappaQueue
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
                    webBuilder
                        .UseKestrel(options =>
                        {
                            options.Listen(IPAddress.Loopback, 5000);  // http:localhost:5000
                            options.Listen(IPAddress.Any, 80);         // http:*:80
                         /*   options.Listen(IPAddress.Loopback, 443, listenOptions =>
                            {
                                listenOptions.UseHttps("certificate.pfx", "password");
                            }); */
                        })
                        .UseStartup<Startup>();
                });
    }
}
