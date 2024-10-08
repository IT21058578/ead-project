using api.Annotations.Validation;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api.Configuratons
{
    /// <summary>
    /// The ValidObjectIdFilter class is responsible for applying a filter to the OpenApiSchema object.
    /// </summary>
    /// 
    /// <remarks>
    /// The ValidObjectIdFilter class implements the ISchemaFilter interface and is used to apply a filter to the schema.
    /// It checks if the MemberInfo has the ValidObjectIdAttribute and if so, it adds the "isObjectId" extension to the schema
    /// and sets the Example property to a sample object ID.
    /// </remarks>
    public class ValidObjectIdFilter : ISchemaFilter
    {
        // This method is called by the Apply method to apply the filter to the schema.
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var attr = context.MemberInfo?.CustomAttributes
            .Where(x => x.AttributeType.Name == nameof(ValidObjectIdAttribute))
            .FirstOrDefault();
            if (attr != null)
            {
                schema.Extensions.Add("isObjectId", new OpenApiBoolean(true));
                schema.Example = new OpenApiString("66f85591eb223db6f583ba43");
            }
        }
    }
}