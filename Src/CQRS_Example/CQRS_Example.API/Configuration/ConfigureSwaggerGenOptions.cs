using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CQRS_Example.API.Configuration
{
    public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(string? name, SwaggerGenOptions options)
        {
            foreach(var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }

            options.ConfigureForNodaTime();
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.SchemaFilter<GenericClassFilter>();
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "CQRS Example API",
                Version = description.ApiVersion.ToString(),
                Description= "This Api implements a sample CQRS implementation, it is for demo purposes only"
            };

            if (description.IsDeprecated)
            {
                info.Description += "This api version is deprectated, please use of of the newer versions if available";
            }

            return info;
        }
    }
}
