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
    public class ValidObjectIdFilter : ISchemaFilter
    {
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