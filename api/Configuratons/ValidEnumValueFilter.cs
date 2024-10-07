using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Annotations.Validation;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api.Configuratons
{
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