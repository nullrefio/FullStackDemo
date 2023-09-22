using Microsoft.AspNetCore.Http.Features;
using Nullref.FullStackDemo.API.ServiceInstallers;
using Nullref.FullStackDemo.API.Services;
using System.Reflection;

namespace Nullref.FullStackDemo.API
{
    public sealed class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        private IConfiguration Configuration { get; }

        internal const string MyAllowSpecificOrigins = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = long.MaxValue;
                o.MemoryBufferThreshold = 5000000;
            });
            services.AddHttpContextAccessor();

            //Fake data context. make singleton so it mimics a real database.
            //data is persisted while app lives
            services.AddSingleton<IDataContext>(config => new DataContext());

            SwaggerInstaller.Run(services);

            services.AddCors(options =>
            {
                var origins = ("http://localhost:4200,https://localhost:4200,http://localhost,https://localhost")
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                if (!origins.Any())
                    throw new Exception("Setting 'CorsOrigins' must be defined.");

                options.AddPolicy(Startup.MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(origins.ToArray());
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.WithExposedHeaders("Location");
                });
            });

            services.AddHeaderPropagation();

            services.Scan(selector => selector
                    .FromCallingAssembly()
                    .AddClasses(
                        classSelector =>
                            classSelector.AssignableTo<IWidgetService>()
                    )
                    .AsImplementedInterfaces()
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHeaderPropagation();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/swagger";
                c.SwaggerEndpoint("v1/swagger.json", "Typescript Compiled API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
        }
    }

    static class AppVersionInfo
    {
        public static string AssemblyInfoVersion =>
            GetAssemblyAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        private static T GetAssemblyAttribute<T>() where T : Attribute =>
            typeof(AppVersionInfo).Assembly.GetCustomAttribute<T>();
    }
}
