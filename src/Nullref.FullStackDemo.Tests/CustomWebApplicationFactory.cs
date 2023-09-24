using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Nullref.FullStackDemo.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // clear configuration sources to avoid too many file watchers on the appsettings.*.json
                // shouldn't be any need to configure during unit test phase(?)
                config.Sources.Clear();

                // add back the json configuration, this time _without_ watchers on the files
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

                // Add non-null database connection string
                var builder = new ConfigurationBuilder();
                builder.AddCommandLine(new string[] { });
                var configuration = builder.Build();
                config.AddConfiguration(configuration);
            });

            builder.ConfigureLogging(loggingBuilder =>
            {
                // clear all configured loggers for tests
                loggingBuilder.ClearProviders();
            });
        }
    }
}
