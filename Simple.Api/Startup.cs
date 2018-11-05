using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Xml;
using Autofac.Extensions.DependencyInjection;
using Simple.Api.AuthHelper;
using Swashbuckle.AspNetCore.Swagger;
using Simple.Api.SwaggerHelper;

namespace Simple.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
           var connectionString = Configuration.GetSection("AppSettings:SqlServerConnection").Value; //获取数据库链接字符串
            services.AddDbContext<DbContext>(option =>
            {
                //option.UseSqlServer(SqlSugarBaseDb.ConnectionString, db => db.UseRowNumberForPaging());
                option.UseSqlServer(connectionString);
            });
            //services.AddSingleton(SqlSugarBaseDb.ConnectionString);
            //services.AddTransient<IStudentSubscriberService, StudentService>();

            //services.AddCap(c =>
            //{
            //    c.UseSqlServer(SqlSugarBaseDb.ConnectionString);
            //    c.UseRabbitMQ(cfg =>
            //    {
            //        cfg.HostName = "localhost";
            //        cfg.VirtualHost = "/";
            //        cfg.Port = 15672;
            //        cfg.UserName = "sa";
            //        cfg.Password = "123456";
            //    });
            //    c.DefaultGroup = "default-group-name";
            //    c.UseDashboard();
            //    c.UseDiscovery(d =>
            //    {
            //        d.DiscoveryServerHostName = "localhost";
            //        d.DiscoveryServerPort = 8500;
            //        d.CurrentNodeHostName = "localhost";
            //        d.CurrentNodePort = 5800;
            //        d.NodeId = 1;
            //        d.NodeName = "CAP No.1 Node";
            //    });
            //});
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
            #region Swagger
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info()
                {
                    Version = "1.0.0",
                    Title = "Simple WebApi",
                    Description = "",
                    TermsOfService = "",
                    Contact = new Contact { Name = "Simple", Email = "2141576@qq.com", Url = "" }
                });
                //添加对控制器的标签(描述)
                s.DocumentFilter<SwaggerDocTag>();
                //添加接口方法描述
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "Simple.Api.xml");
                s.IncludeXmlComments(xmlPath, true);
                //手动高亮
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                s.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                s.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });
            #endregion

            #region  Token服务注册
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("AdminOrClient", policy => policy.RequireRole("AdminOrClient").Build());
            });
            #endregion

            #region autofac
            //实例化容器
            var builder = new ContainerBuilder();
            //获取项目路径
            var dllPath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            //注册需要通过反射创建的组件
            //builder.RegisterType<StudentService>().As<IStudentService>();
            //var assemblysRepository = Assembly.Load("Ourstory.Service");
            var assemblysService = Assembly.LoadFile(Path.Combine(dllPath, "Simple.Services.dll"));
            builder.RegisterAssemblyTypes(assemblysService).AsImplementedInterfaces();
            //var assemblysRepository = Assembly.Load("Ourstory.Repository");
            var assemblysRepository = Assembly.LoadFile(Path.Combine(dllPath, "Simple.Repositories.dll"));
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();
            //将services填充到Autofac容器生成器中
            builder.Populate(services);
            //使用已进行的组件登记创建新容器
            var applicationContainer = builder.Build();
            #endregion

            return new AutofacServiceProvider(applicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1"));
                #endregion
            }
            app.UseMiddleware<JwtTokenAuth>();
            app.UseMvc();
            //app.UseCap();
        }
    }
}
