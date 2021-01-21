using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using DB.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Models.CommonModel;
using WebApi1.Extension.ExtensionModel;
using WebApi1.Extension.ExtensionModel.ConfigOptions;
using WebApi1.Extension.Middleware.ErrorHandler;

namespace WebApi1
{

    public class Startup
    {
        public ILifetimeScope AutofacContainer { get; private set; }
        private IConfiguration config;
        public Startup(IConfiguration configuration)
        {
            config = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<MyOptions>(config);//映射Config键值对到Options类中

            // Create a container-builder and register dependencies

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //配置允许跨域
            services.AddCors(option =>
            {
                option.AddPolicy("*", build =>
                {
                    build.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin();
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", jwtBearOptions =>
            {
                jwtBearOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("shenlilinAPPSecret")),

                    ValidateIssuer = true,
                    ValidIssuer = "WEBAPI1",

                    ValidateAudience = true,
                    ValidAudience = "Client1",

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //允许5分钟的偏差
                };
            });

            //services.AddDbContext<SchoolContext>(options =>
            //    {
            //        options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            //    });

            services.AddDbContext<PetshopContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            //使用Autofac替代net core 默认DI
            var builder = new ContainerBuilder();
            builder.Populate(services);

            ConfigureContainer(builder);

            AutofacContainer = builder.Build();

            return new AutofacServiceProvider(AutofacContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, ILoggerProvider loggerProvider)
        {
            Console.WriteLine(Environment.CurrentDirectory);

            #region

            //Console.WriteLine(config.GetConnectionString("DefaultConnection"));
            //Console.WriteLine(config.GetSection("Section0").GetChildren().ToList()[1].Value);
            //Console.WriteLine(config.GetSection("Section0").GetSection("key0").Value);
            //Console.WriteLine(config.GetSection("Section1:key2").Value);
            //Console.WriteLine(config.GetSection("section0:key0").Exists());

            //ILogger logger = loggerFactory.CreateLogger<Startup>();
            //logger.LogInformation("111");//循环调用每个LoggingProvider

            //ILogger logger2 = loggerProvider.CreateLogger(nameof(Startup));//调用最后一个注册的LoggingProvider
            //logger2.LogInformation("222");

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}

            //app.UseStaticFiles(); // For the wwwroot folder

            ////启用目录浏览
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
            //    RequestPath = "/MyImages"
            //});

            //UseStaticFiles UseDefaultFiles：添加默认文档，URL重写 UseDirectoryBrowser三者集合

            #endregion

            app.UseCors("*");

            app.UseFileServer(env.IsDevelopment());//生产环境禁用目录浏览

            var p = Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles");
            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }

            var cachePeriod = env.IsDevelopment() ? "600" : "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(p),
                RequestPath = "/StaticFiles",
                OnPrepareResponse = ctx =>
                {
                    //设置响应缓存cache
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });

            app.UseErrorHandling();

            app.UseAuthentication();


            //app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}",
                    defaults: new { controller = "Values", action = "Get" }
                    );
            });

            //app.UseWelcomePage();
        }

        /// <summary>
        /// 配置个性化服务
        /// </summary>
        /// <param name="builder"></param>
        void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<mJsonResult>().AsSelf().InstancePerLifetimeScope();

            //程序集可以不用引用，直接将dll拷贝到目录下即可
            var assembly = Assembly.Load("Service");
            var assemblyAccess = Assembly.Load("DataAccess");

            //约定所有服务名称以Service字符串结尾
            builder.RegisterAssemblyTypes(assembly) //注册程序集
                .Where(t => t.Name.EndsWith("Service"))//匹配所有满足条件的类型
                .AsSelf()//为自身提供服务
                .InstancePerLifetimeScope();//Request LiftTime


            builder.RegisterAssemblyTypes(assemblyAccess)
                .Where(t => t.Name.EndsWith("Repository") && t.IsInterface == false)
                .AsImplementedInterfaces()//为所有继承自的接口服务提供实现
                .InstancePerLifetimeScope();
        }
    }
}
