using EmployeeManagement.Security;
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
using MockSchoolManagement.Infrastructure;
using MockSchoolManagement.Infrastructure.Data;
using MockSchoolManagement.Infrastructure.Repositories;
using MockSchoolManagement.Models;
using MockSchoolManagement.Security;
using MockSchoolManagement.Security.CustomTokenProvider;
using NetCore.AutoRegisterDi;
using System;

namespace MockSchoolManagement
{
    public class Startup
    {
        private IWebHostEnvironment _env;
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this._env = env;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            //使用Sqlserver数据，通过IConfiguration访问去获取，自定义名称的"MockStudentDBConnection"作为我们的链接字符串
            services.AddDbContextPool<AppDbContext>(options =>
            options.UseLazyLoadingProxies().
            UseSqlServer(_configuration.GetConnectionString("MockStudentDBConnection")));

            var builder = services.AddControllersWithViews(config =>
            {
                //注释后开启全局验证
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter());
            }).AddXmlSerializerFormatters();

            builder.AddRazorRuntimeCompilation();

            #region 授权验证

            services.AddAuthentication()
              .AddMicrosoftAccount(microsoftOptions =>
              {
                  microsoftOptions.ClientId = _configuration["Authentication:Microsoft:ClientId"];
                  microsoftOptions.ClientSecret = _configuration["Authentication:Microsoft:ClientSecret"];
              }).AddGitHub(options =>
              {
                  options.ClientId = _configuration["Authentication:GitHub:ClientId"];
                  options.ClientSecret = _configuration["Authentication:GitHub:ClientSecret"];
              });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
                //通过自定义的CustomEmailConfirmation名称来覆盖旧有token名称，是它与AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation")关联在一起
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders()
                    .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromDays(3));

            services.Configure<DataProtectionTokenProviderOptions>(
               o => o.TokenLifespan = TimeSpan.FromHours(10)//令牌有效期10秒
               );

            #endregion 授权验证

            #region 策略授权的声明

            //services.Configure<DataProtectionTokenProviderOptions>(
            // o => o.TokenLifespan = TimeSpan.FromHours(2)
            // );

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

            #endregion 策略授权的声明

            #region 进行依赖注入服务的容器

            //自动注入服务到依赖注入容器
            services.RegisterAssemblyPublicNonGenericClasses()
             .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            services.AddSingleton<DataProtectionPurposeStrings>();

            //services.AddScoped<IStudentRepository, SQLStudentRepository>();
            //services.AddScoped<IStudentService, StudentService>();
            //services.AddScoped<ICourseService, CourseService>();

            services.AddSingleton<IAuthorizationHandler,
        CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler,
       SuperAdminHandler>();

            services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));

            #endregion 进行依赖注入服务的容器

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
        public void Configure(IApplicationBuilder app)
        {
            //如果环境是Development serve Developer Exception Page
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //否则提供友好错误页面联系信息
            else if (_env.IsStaging() || _env.IsProduction() || _env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            //数据初始化
            app.UseDataInitializer();

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

        // app.UseMvcWithDefaultRoute(); app.UseMvc();

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