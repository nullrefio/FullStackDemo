using Nullref.FullStackDemo.API;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Xunit;

namespace Nullref.FullStackDemo.Tests
{
    public class ApiSpecTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private const string AngularProjectFolder = "Nullref.FullStackDemo.Frontend";

        public ApiSpecTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task OutputApi()
        {
            var response = await _client.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri("api/swagger/v1/swagger.json", UriKind.Relative),
                Headers =
                {
                    // force the host header to a known value to minimize churn in generated swagger/client
                    Host = "localhost:65378"
                }
            });
            var responseJson = await response.Content.ReadAsStringAsync();

            var solutionDirectory = GetSolutionDirectory();
            Assert.NotEmpty(solutionDirectory);

            //Create the swagger file
            var swaggerChanged = true;
            var outputPath = Path.Combine(solutionDirectory, AngularProjectFolder, "swagger.json");
            if (File.Exists(outputPath))
            {
                var current = await File.ReadAllTextAsync(outputPath);
                if (string.Compare(current, responseJson, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) == 0)
                    swaggerChanged = false;
            }
            if (swaggerChanged)
                await File.WriteAllTextAsync(outputPath, responseJson);

            GenerateTypescriptApi();
        }

        private void GenerateTypescriptApi()
        {

            var specPath = Path.Combine(GetSolutionDirectory(), AngularProjectFolder);
            var info = new ProcessStartInfo("npm")
            {
                Arguments = "run generate",
                CreateNoWindow = false,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                WorkingDirectory = specPath,
            };

            var process = Process.Start(info);
            process.EnableRaisingEvents = true;
            var exited = process.WaitForExit(1000 * 20); // wait up to 20 seconds
        }

        private static string GetSolutionDirectory()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory);

            // walk up the directory path until we get to the project directory
            // assumes the current assembly has the same name as it's directory
            while (!string.Equals(directoryInfo.Name, assemblyName))
            {
                Assert.NotNull(directoryInfo.Parent);
                directoryInfo = directoryInfo.Parent;
            }

            // one more to get from project dir to solution dir
            directoryInfo = directoryInfo.Parent;

            return directoryInfo?.FullName;
        }
    }

    file static class Extensions
    {
        public static IEnumerable<string> SplitLines(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Array.Empty<string>();
            }

            return data.Replace("\r\n", "\n").Split("\n");
        }
    }
}
