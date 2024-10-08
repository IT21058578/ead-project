using api.Annotations.Validation;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api.Configuratons
{
    /// <summary>
    /// The ValidEnumValueFilter class is responsible for applying a filter to the OpenApiSchema based on the ValidEnumValueAttribute.
    /// </summary>
    /// 
    /// <remarks>
    /// The ValidEnumValueFilter class implements the ISchemaFilter interface and provides a method to apply the filter to the schema.
    /// It checks if the member has the ValidEnumValueAttribute and sets the schema's Example property to the first value of the enum type.
    /// </remarks>
    public class ValidEnumValueFilter : ISchemaFilter
    {
        //  This method is called by the Apply method to apply the filter to the schema.
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var attr = context.MemberInfo?.CustomAttributes
                .Where(x => x.AttributeType.Name == nameof(ValidEnumValueAttribute))
                .FirstOrDefault();

            if (attr != null)
            {
                // Get the enum type from the attribute constructor arguments
                var enumType = attr.ConstructorArguments.FirstOrDefault().Value as Type;
                if (enumType != null)
                {
                    schema.Example = new OpenApiString(Enum.GetNames(enumType).GetValue(0).ToString());
                }
            }
        }
    }
}