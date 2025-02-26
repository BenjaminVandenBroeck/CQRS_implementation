using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CQRS_Example.API.Configuration
{
    public class GenericClassFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (type.IsGenericType == false)
            {
                return;
            }

            schema.Title = $"{type.Name[..^2]}<{type.GenericTypeArguments[0].Name}>";
        }
    }
}
