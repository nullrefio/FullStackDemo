using MicroElements.Swashbuckle.NodaTime;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NodaTime;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nullref.FullStackDemo.API.ServiceInstallers
{
    public static class SwaggerInstaller
    {
        public static void Run(IServiceCollection services)
        {
            // support api versioning
            services.AddApiVersioning();
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<AdditionalAttributesFilter>();
                c.UseAllOfToExtendReferenceSchemas();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Nullref FullStackDemo API",
                    Description = "This is a REST API interface.",
                    Version = AppVersionInfo.AssemblyInfoVersion,
                });

                c.EnableAnnotations();
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

                // support NodaTime
                var exampleDate = new DateTime(2018, 1, 9, 14, 4, 6, DateTimeKind.Utc);
                c.ConfigureForNodaTime(schemaExamples: new SchemaExamples(DateTimeZoneProviders.Tzdb, exampleDate, DateTimeZoneProviders.Tzdb.GetZoneOrNull("EST").ToString()));
            });

        }
    }

    public class AdditionalAttributesFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            //If there is no member info then there is nothing to do
            if (context?.MemberInfo is null) return;

            //Do not generate for some models as not needed
            var check1 = context.MemberInfo.ReflectedType.IsAssignableTo(typeof(IPagingCriteriaModel));
            var check2 = context.MemberInfo.ReflectedType.IsAssignableTo(typeof(IResponseModel));
            if (check1 || check2) return;

            var displayName = context?
                .MemberInfo?
                .GetCustomAttributes(false)
                .OfType<DisplayNameAttribute>()
                .OrEmpty()
                .FirstOrDefault();
            if (displayName is null)
                schema.Extensions.Add("displayName", new OpenApiString(context?.MemberInfo?.Name));
            else
                schema.Extensions.Add("displayName", new OpenApiString(displayName.DisplayName));

            var description = context?
                .MemberInfo?
                .GetCustomAttributes(false)
                .OfType<DescriptionAttribute>()
                .OrEmpty()
                .FirstOrDefault();
            if (description is null)
                schema.Extensions.Add("description", new OpenApiString(string.Empty));
            else
                schema.Extensions.Add("description", new OpenApiString(description.Description));

            var allowSort = (context?
                .MemberInfo?
                .GetCustomAttributes(false)
                .OfType<SortableAttribute>()
                .OrEmpty()
                .FirstOrDefault() == null);
            if (allowSort)
                schema.Extensions.Add("allowSort", new OpenApiBoolean(false));
            else
                schema.Extensions.Add("allowSort", new OpenApiBoolean(true));
        }
    }

}
