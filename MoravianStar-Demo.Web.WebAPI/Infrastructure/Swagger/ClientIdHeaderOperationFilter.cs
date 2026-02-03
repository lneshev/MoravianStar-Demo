using Microsoft.OpenApi;
using MoravianStar_Demo.Common.Core.Constants;
using MoravianStar_Demo.Web.WebAPI.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace MoravianStar_Demo.Web.WebAPI.Infrastructure.Swagger
{
    public class ClientIdHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (IsClientIdHeaderRequired(context))
            {
                operation.Parameters ??= new List<IOpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HTTPHeaderConstants.ClientId,
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.String
                    }
                });
            }
        }

        private bool IsClientIdHeaderRequired(OperationFilterContext context)
        {
            if (context.MethodInfo.DeclaringType.BaseType.IsGenericType)
            {
                return typeof(ClientEntityRestController<,,,>).IsAssignableFrom(context.MethodInfo.DeclaringType.BaseType.GetGenericTypeDefinition());
            }
            return false;
        }
    }
}