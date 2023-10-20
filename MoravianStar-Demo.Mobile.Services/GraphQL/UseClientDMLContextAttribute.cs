using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoravianStar_Demo.Common.Core.Constants;
using MoravianStar_Demo.Common.Core.Resources;
using MoravianStar_Demo.Persistence.DbContexts;
using System;
using System.Reflection;

namespace MoravianStar_Demo.Mobile.Services.GraphQL
{
    /// <summary>
    /// This attribute registers a middleware for <see cref="ClientDMLContext"/> that sets the client's database connection string by using the client's Id from the global context.
    /// </summary>
    public class UseClientDMLContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.Extend().Definition.MiddlewareDefinitions.Add(new(next => async context =>
            {
                var configuration = context.Services.GetRequiredService<IConfiguration>();
                var clientIdString = context.GetGlobalValue<string>(HTTPHeaderConstants.ClientId);

                if (!string.IsNullOrWhiteSpace(clientIdString) && int.TryParse(clientIdString, out int clientId))
                {
                    var connectionString = string.Format(configuration["ConnectionStrings:Client"], clientId);
                    var dbContext = context.Services.GetRequiredService<ClientDMLContext>();
                    dbContext.Database.SetConnectionString(connectionString);
                    await next(context);
                }
                else
                {
                    throw new ArgumentNullException(HTTPHeaderConstants.ClientId, Strings.HTTPHeaderClientIdIsRequired);
                }
            }));
        }
    }
}