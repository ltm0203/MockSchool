using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockSchoolManagement.DataRepositories;
using MockSchoolManagement.Infrastructure;
using MockSchoolManagement.Models;
using MockSchoolManagement.Security;
using System;

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
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddHttpContextAccessor();


            //使用Sqlserver数据，通过IConfiguration访问去获取，自定义名称的"MockStudentDBConnection"作为我们的链接字符串
            services.AddDbContextPool<AppDbContext>(
            options => options.UseSqlServer(_configuration.GetConnectionString("MockStudentDBConnection")));

            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                              .RequireAuthenticatedUser()
                                              .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }
            ).AddXmlSerializerFormatters();

            services.AddAuthentication()
             
                .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = _configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = _configuration["Authentication:Microsoft:ClientSecret"];
            }).AddGitHub(options =>
            {
                options.ClientId = _configuration["Authentication:GitHub:ClientId"];
                options.ClientSecret = _configuration["Authentication:GitHub:ClientSecret"];

            })       
            ;

       



            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            // 策略结合声明授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                   policy => policy.RequireClaim("Delete Role"));

                options.AddPolicy("AdminRolePolicy",
                   policy => policy.RequireRole("Admin"));

                //策略结合多个角色进行授权
                options.AddPolicy("SuperAdminPolicy", policy =>
                  policy.RequireRole("Admin", "User", "SuperManager"));

                options.AddPolicy("EditRolePolicy", policy =>
         policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
            });


            ////策略结合角色授权
            //services.AddAuthorization(options =>
            //{

            //});


            //services.AddAuthorization(options =>
            //{

            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("EditRolePolicy", policy =>
            //        policy.RequireAssertion(context => AuthorizeAccess(context)));
            //});


            //         services.AddAuthorization(options =>
            //         {
            //             options.AddPolicy("EditRolePolicy", 
            //                 policy => policy.RequireAssertion(context =>
            //                 context.User.IsInRole("Admin") &&
            //                 context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
            //                 context.User.IsInRole("Super Admin")
            //));
            //         });


            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("EditRolePolicy", 
            //        policy => policy.RequireClaim("Edit Role","true"));
            //});


            services.AddScoped<IStudentRepository, SQLStudentRepository>();

            services.AddSingleton<IAuthorizationHandler,
        CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler,
       SuperAdminHandler>();
           
            services.ConfigureApplicationCookie(options =>
            {
                //修改拒绝访问的路由地址
                options.AccessDeniedPath = new PathString("/Admin/AccessDenied");
                //修改登录地址的路由
             //   options.LoginPath = new PathString("/Admin/Login");  
                //修改注销地址的路由
             //   options.LogoutPath = new PathString("/Admin/LogOut");
                //统一系统全局的Cookie名称
                options.Cookie.Name = "MockSchoolCookieName";
                // 登录用户Cookie的有效期 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);             
                //是否对Cookie启用滑动过期时间。
                options.SlidingExpiration = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //如果环境是Development serve Developer Exception Page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //否则提供友好错误页面联系信息
            else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            //使用纯静态文件支持的中间件，而不使用带有终端中间件
            app.UseStaticFiles();

            //添加身份认证中间件
            app.UseAuthentication();
            app.UseRouting();
            //添加授权中间件
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //授权访问
        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("Admin") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                    context.User.IsInRole("Super Admin");
        }
        // app.UseMvcWithDefaultRoute();
        //  app.UseMvc();

        //否则提供友好错误页面联系信息
        //else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
        //{
        //    app.UseExceptionHandler("/Error");
        //}

        //routeBuilder.MapGet("hello/{name}", context =>
        //{
        //    var name = context.GetRouteValue("name");
        //    // The route handler when HTTP GET "hello/<anything>" matches
        //    // To match HTTP GET "hello/<anything>/<anything>,
        //    // use routeBuilder.MapGet("hello/{*name}"
        //    return context.Response.WriteAsync($"Hi, {name}!");
        //});

        //var trackPackageRouteHandler = new RouteHandler(context =>
        //{
        //    var routeValues = context.GetRouteData().Values;
        //    return context.Response.WriteAsync(
        //        $"Hello! Route values: {string.Join(", ", routeValues)}");
        //});

        //var routeBuilder = new RouteBuilder(app, trackPackageRouteHandler);

        //routeBuilder.MapRoute(
        //    "Track Package Route",
        //    "package/{operation:regex(^track|create$)}/{id:int}");

        //routeBuilder.MapGet("hello/{name}", context =>
        //{
        //    var name = context.GetRouteValue("name");
        //    // The route handler when HTTP GET "hello/<anything>" matches
        //    // To match HTTP GET "hello/<anything>/<anything>,
        //    // use routeBuilder.MapGet("hello/{*name}"
        //    return context.Response.WriteAsync($"Hi, {name}!");
        //});

        //var routes = routeBuilder.Build();

        //app.UseRouter(routes);

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapControllerRoute(
        //        name: "default",
        //        pattern: "{controller=Home}/{action=Index}/{id?}");
        //});
    }
}