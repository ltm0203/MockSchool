using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MockSchoolManagement
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services )
        {     
          

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            //app.Run(async (context) =>
            //{
            //    context.Response.ContentType = "text/plain; charset=utf-8"; //防止乱码

            //    await context.Response.WriteAsync("从第一个中间件中打印Hello World");
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("从第二个中间件中打印Hello World");
            //});


            //app.Use(async (context, next) =>
            //{  
            //    await next();
            //});

            //app.Run(async (context) =>
            //{
            //    context.Response.ContentType = "text/plain; charset=utf-8";
            //    await context.Response.WriteAsync("从第二个中间件中打印Hello World");
            //});
                               
                
          
                app.Use(async (context, next) =>
                {
                    logger.LogInformation("MW1:传入请求");
                    await next();
                    logger.LogInformation("MW1:传出响应");
                });

                app.Use(async (context, next) =>
                {
                    logger.LogInformation("MW2: 传入请求");
                    await next();
                    logger.LogInformation("MW2: 传出响应");
                });

                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("MW3: 处理请求并生成响应");
                    logger.LogInformation("MW3: 处理请求并生成响应");
                });
            } 
        }
    }
