//using DataAnnotatedModelValidations;
using GraphQL.Server.Ui.Voyager;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoravianStar.WebAPI.GraphQL.Validation;
using MoravianStar_Demo.Mobile.Services.GraphQL;
using MoravianStar_Demo.Mobile.Services.GraphQL.Mutations.Test;
using MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test;
using MoravianStar_Demo.Mobile.WebAPI.Infrastructure.HttpRequestInterceptors;
using MoravianStar_Demo.Persistence.DbContexts;

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
                .AddPooledDbContextFactory<DataLayer_SystemContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionStrings:TestSystem"], sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                });

            services
                .AddDbContextFactory<DataLayer_ClientDMLContext>(options =>
                {
                    options.UseSqlServer(".", sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                });

            services
                .AddGraphQLServer()
                .SetPagingOptions(new PagingOptions()
                {
                    IncludeTotalCount = true
                })
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .AddSpatialTypes()
                .AddSpatialProjections()
                .AddSpatialFiltering()
                .AddHttpRequestInterceptor<DataLayerHttpRequestInterceptor>()
                .AddMutationConventions(new MutationConventionOptions()
                {
                    ApplyToAllMutations = false
                })
                .AddDataAnnotationsValidator()
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

            app.UseGraphQLVoyager(new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql"
            }, "/graphql-voyager");
        }
    }
}