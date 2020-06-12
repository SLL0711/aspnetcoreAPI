using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApi1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (var hostNew = WebHost.Start("http://localhost:8000", app =>
            //{
            //    app.MapGet("/HELLO/{name}",
            //        async (request, response, routedata) =>
            //        {
            //            await response.WriteAsync(routedata.Values["name"].ToString());
            //        });
            //}))
            //{
            //    Console.WriteLine("Use Ctrl-C to shut down the host...");
            //    hostNew.WaitForShutdown();
            //}
            //return;

            var host = CreateWebHostBuilder(args).Build();

            using (var x = host.Services.CreateScope())
            {
                var provider = x.ServiceProvider;
            }

            // The log category is a string that is associated with each log.
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            System.Linq.Enumerable.Range(0, 1).ToList().ForEach(item =>
             {
                 logger.LogInformation("seed the database...info");
             });

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices(services => { services.AddAutofac(); })
                //.PreferHostingUrls(false)
                //.UseUrls("http://*:8080")
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddLog4Net(Directory.GetCurrentDirectory() + "/log4net.config");
                });
        //.UseSetting(WebHostDefaults.ApplicationKey, "CustomApplicationName")//修改ApplicationKey默认值
        //.UseEnvironment("Development")//设置Environment初始值
        //.UseSetting("https_port", "8080")

        //可以替代Startup类中的配置
        //.Configure(app =>
        //{
        //    app.UseMvc(routes =>
        //    {
        //        routes.MapRoute(
        //            name: "default",
        //            template: "api/{controller}/{action}/{id?}",
        //            defaults: new { controller = "Values", action = "Get" }
        //        );
        //    });
        //})
        //.ConfigureServices(services =>
        //{
        //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        //});
    }
}
