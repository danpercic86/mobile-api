using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace itec_mobile_api_final.Helpers
{
    public class ReadOnlyFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
            {
                return;
            }

            foreach (var schemaProperty in schema.Properties)
            {
                var property = context.SystemType.GetProperty(schemaProperty.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    var attr = (ReadOnlyAttribute)property.GetCustomAttributes(typeof(ReadOnlyAttribute), false).SingleOrDefault();
                    if (attr != null && attr.IsReadOnly)
                    {
                        // https://github.com/swagger-api/swagger-ui/issues/3445#issuecomment-339649576
                        if (schemaProperty.Value.Ref != null)
                        {
                            schemaProperty.Value.AllOf = new List<Schema>()
                            {
                                new Schema()
                                {
                                    Ref = schemaProperty.Value.Ref
                                }
                            };
                            schemaProperty.Value.Ref = null;
                        }

                        schemaProperty.Value.ReadOnly = true;
                    }
                }
            }
        }
    }
}