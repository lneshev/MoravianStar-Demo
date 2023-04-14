using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoravianStar_Demo.Persistence.DbContexts;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MoravianStar_Demo.Mobile.Services.GraphQL
{
    /// <summary>
    /// This attribute registers a middleware for <see cref="DataLayer_ClientDMLContext"/> that sets the client's database connection string by using the client's Id from the global context
    /// and registers the correct repository, so that it can be used.
    /// </summary>
    public class UseDataLayerDbContextAttribute : UseDbContextAttribute
    {
        private readonly Type dbContextType;

        public UseDataLayerDbContextAttribute(Type dbContextType, [CallerLineNumber] int order = 0) : base(dbContextType, order)
        {
            this.dbContextType = dbContextType;
        }

        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            base.OnConfigure(context, descriptor, member);

            if (typeof(DataLayer_ClientDMLContext).IsAssignableFrom(dbContextType))
            {
                descriptor.Extend().Definition.MiddlewareDefinitions.Add(new(next => async context =>
                {
                    var configuration = context.Services.GetRequiredService<IConfiguration>();
                    var clientId = context.GetGlobalValue<int?>("ClientId");

                    if (clientId.HasValue)
                    {
                        var connectionString = string.Format(configuration["ConnectionStrings:TestClient"], clientId);

                        var dbContextServiceName = typeof(DataLayer_ClientDMLContext).FullName ?? typeof(DataLayer_ClientDMLContext).Name;
                        var dbContext = context.GetLocalValue<DataLayer_ClientDMLContext>(dbContextServiceName);

                        dbContext.Database.SetConnectionString(connectionString);
                        try
                        {
                            await next(context);
                        }
                        finally
                        {
                            // Reset the connection string, just in case
                            dbContext.Database.SetConnectionString(null);
                        }
                    }
                    else
                    {
                        await next(context);
                    }
                }));

                RegisterRepository<ClientRepository, DataLayer_ClientDMLContext>(descriptor);
            }
            else if (typeof(DataLayer_SystemContext).IsAssignableFrom(dbContextType))
            {
                RegisterRepository<SystemRepository, DataLayer_SystemContext>(descriptor);
            }
        }

        private static void RegisterRepository<TRepository, TDbContext>(IObjectFieldDescriptor descriptor)
            where TRepository : IEntityRepository<TDbContext>
            where TDbContext : DbContext
        {
            descriptor.Extend().Definition.MiddlewareDefinitions.Add(new(next => async context =>
            {
                var dbContextServiceName = typeof(TDbContext).FullName ?? typeof(TDbContext).Name;
                var dbContext = context.GetLocalValue<TDbContext>(dbContextServiceName);

                var repository = (TRepository)Activator.CreateInstance(typeof(TRepository), dbContext);
                var repositoryServiceName = repository.GetType().FullName;

                try
                {
                    context.SetLocalValue(repositoryServiceName, repository);
                    await next(context);
                }
                finally
                {
                    context.RemoveLocalValue(repositoryServiceName);
                }
            }));
        }
    }
}