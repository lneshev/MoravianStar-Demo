using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoravianStar.Dao;
using MoravianStar.Extensions;
using MoravianStar.Settings;
using MoravianStar.WebAPI.Attributes;
using MoravianStar.WebAPI.Extensions;
using MoravianStar.WebAPI.JsonConverters;
using MoravianStar.WebAPI.ModelBinders;
using MoravianStar.WebAPI.Swagger;
using MoravianStar.WebAPI.Transformers;
using MoravianStar_Demo.Common.Core.Configuration;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Enums.Test;
using MoravianStar_Demo.Common.Core.Resources;
using MoravianStar_Demo.Common.Services.Test;
using MoravianStar_Demo.Persistence.DbContexts;
using MoravianStar_Demo.Web.Core.Models.Test;
using MoravianStar_Demo.Web.Services.Test;
using MoravianStar_Demo.Web.WebAPI.Infrastructure.Attributes;
using MoravianStar_Demo.Web.WebAPI.Infrastructure.Constants;
using NetTopologySuite.IO;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var applicationsConfigSection = configuration.GetSection(nameof(ApplicationsConfiguration));
            services.Configure<ApplicationsConfiguration>(options =>
            {
                options.JobWebAPI = applicationsConfigSection.GetSection(nameof(ApplicationsConfiguration.JobWebAPI)).Get<ApplicationConfiguration>();
            });

            services.AddControllers(options =>
                    {
                        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                        options.Filters.Add<ClientDMLContextFilterAttribute>();
                        options.Filters.Add<ValidateModelStateAttribute>();
                        options.AddCustomSimpleTypeModelBinderProvider();
                    })
                    .AddControllersAsServices()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Converters.Add(new CustomStringTypeJsonConverter());
                        foreach (var geoJsonConverter in GeoJsonSerializer.CreateDefault().Converters)
                        {
                            options.SerializerSettings.Converters.Add(geoJsonConverter);
                        }
                    });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // This options is set to "true", because the logic in ValidateModelStateAttribute 
                // won't be executed for controllers marked with ApiControllerAttribute
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.DocumentFilter<HideInDocsFilter>();
            });

            services.AddAuthorization();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyConstants.Default,
                    builder =>
                    {
                        builder
                            .WithOrigins() // TODO: Add urls, that are defined in appsettings.json
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services
                .AddDbContextPool<SystemContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionStrings:System"], sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                });

            // Do not register it as a pooled DbContext, because the connection string is set upon the authentication.
            // It is not tested, but probably DbContext pooling will not work (or will have side effects) in this case.
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

            services.AddScoped<IDbTransaction<SystemContext>, DbTransaction<SystemContext>>();
            services.AddScoped<IDbTransaction<ClientDMLContext>, DbTransaction<ClientDMLContext>>();

            services.AddTransient<IModelsMappingService<AddressModel, AddressEntity>, AddressModelMappingService>();
            services.AddTransient<IModelsMappingService<ClientModel, ClientEntity>, ClientModelMappingService>();
            services.AddTransient<IModelsMappingService<ClientModel2, ClientEntity>, ClientModel2MappingService>();
            services.AddTransient<IModelsMappingService<ClientModel3, ClientEntity>, ClientModel3MappingService>();
            services.AddTransient<IModelsMappingService<BlockModel, BlockEntity>, BlockModelMappingService>();

            services.AddTransient<IEntitySaved<ClientEntity>, ClientSaved>();

            services.AddTransient<IExampleJobSender, ExampleJobSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMoravianStar(env, () =>
            {
                Settings.DefaultDbContextType = typeof(SystemContext);
                Settings.StringResourceTypeForEnums = typeof(Strings);
                Settings.AssemblyForEnums = typeof(ClientStatus).Assembly;
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(CorsPolicyConstants.Default);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseElmah();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}