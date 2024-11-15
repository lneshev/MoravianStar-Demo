using DataAnnotatedModelValidations;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using GraphQL.Server.Ui.Voyager;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoravianStar.Dao;
using MoravianStar.Extensions;
using MoravianStar.GraphQL.Filters;
using MoravianStar.Settings;
using MoravianStar_Demo.Common.Core.Enums.Test;
using MoravianStar_Demo.Common.Core.Resources;
using MoravianStar_Demo.Mobile.Services.GraphQL;
using MoravianStar_Demo.Mobile.Services.GraphQL.Mutations.Test;
using MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test;
using MoravianStar_Demo.Mobile.WebAPI.Infrastructure.HttpRequestInterceptors;
using MoravianStar_Demo.Persistence.DbContexts;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Mobile.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContextPool<SystemContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionStrings:System"], sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                });

            services
                .AddDbContext<ClientDMLContext>(options =>
                {
                    options.UseSqlServer(".", sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                });

            services.AddElmah<SqlErrorLog>(options =>
            {
                options.OnError = async (httpContext, error) =>
                {
                    if (error.Exception != null)
                    {
                        error.StatusCode = error.Exception.GetHttpStatusCode();
                    }
                    await Task.CompletedTask;
                };
                options.ConnectionString = configuration.GetConnectionString("Log");
                options.SqlServerDatabaseSchemaName = "dbo";
                options.SqlServerDatabaseTableName = "Elmah";
                options.OnPermissionCheck = context => true; // context.User.Identity.IsAuthenticated;
            });

            services
                .AddGraphQLServer()
                .ModifyPagingOptions(x =>
                {
                    x.DefaultPageSize = int.MaxValue - 1;
                    x.MaxPageSize = int.MaxValue - 1;
                    x.IncludeTotalCount = true;
                })
                .ModifyCostOptions(x =>
                {
                    x.EnforceCostLimits = false;
                    x.ApplyCostDefaults = false;
                })
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .AddSpatialTypes()
                .AddSpatialProjections()
                .AddSpatialFiltering()
                .AddHttpRequestInterceptor<ClientContextHttpRequestInterceptor>()
                .AddMutationConventions(new MutationConventionOptions()
                {
                    ApplyToAllMutations = false
                })
                .AddDataAnnotationsValidator()
                .AddErrorFilter<ErrorImproverErrorFilter>()
                .AddErrorFilter<ElmahErrorFilter>()
                // Queries:
                .AddQueryType<Query>()
                .AddTypeExtension<AddressQueries>()
                .AddTypeExtension<ClientQueries>()
                .AddTypeExtension<LanguageQueries>()
                .AddTypeExtension<VehicleQueries>()
                .AddTypeExtension<BlockQueries>()
                // Mutations:
                .AddMutationType<Mutation>()
                .AddTypeExtension<ClientMutations>()
                // Subscriptions:
                //.AddSubscriptionType<Subscription>()
                // Types:
                //It is not necessary to create types for all entities, like for LanguageEntity.
                .AddType<ClientType>()
                .AddType<VehicleType>();
                //.RegisterDbContext<SystemContext>(DbContextKind.Synchronized)
                //.RegisterDbContext<ClientDMLContext>(DbContextKind.Synchronized);

            services.AddScoped<IDbTransaction<SystemContext>, DbTransaction<SystemContext>>();
            services.AddScoped<IDbTransaction<ClientDMLContext>, DbTransaction<ClientDMLContext>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Settings.DefaultDbContextType = typeof(SystemContext);
            Settings.StringResourceTypeForEnums = typeof(Strings);
            Settings.AssemblyForEnums = typeof(ClientStatus).Assembly;

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseElmah();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

            app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql"
            });
        }
    }
}