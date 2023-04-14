using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoravianStar.WebAPI.Attributes;
using MoravianStar.WebAPI.JsonConverters;
using MoravianStar.WebAPI.Middlewares;
using MoravianStar.WebAPI.ModelBinders;
using MoravianStar.WebAPI.Swagger;
using MoravianStar.WebAPI.Transformers;
using MoravianStar_Demo.Persistence.DbContexts;
using MoravianStar_Demo.Web.WebAPI.Infrastructure.Attributes;
using MoravianStar_Demo.Web.WebAPI.Infrastructure.Constants;
using NetTopologySuite.IO;

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
                .AddDbContextPool<DataLayer_SystemContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionStrings:TestSystem"], sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                });

            // Do not register it as a pooled DbContext, because the connection string is set upon the authentication.
            // It is not tested, but probably DbContext pooling will not work (or will have side effects) in this case.
            services
                .AddDbContext<DataLayer_ClientDMLContext>(options =>
                {
                    options.UseSqlServer(".", sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                });

            //services.AddTransient<IRepository<DataLayer_SystemContext>, SystemRepository>();
            //services.AddTransient<IRepository<DataLayer_ClientDMLContext>, ClientRepository>();

            //services.AddTransient<ISystemModelService<ClientEntity, int, ClientModel>, ClientModelService>();
            //services.AddTransient<ISystemModelService<ClientEntity, int, ClientModel2>, ClientModel2Service>();
            //services.AddTransient<ISystemModelService<ClientEntity, int, ClientModel3>, ClientModel3Service>();
            //services.AddTransient<ISystemModelService<AddressEntity, Guid, AddressModel>, AddressModelService>();
            //services.AddTransient<IClientModelService<BlockEntity, int, BlockModel>, BlockModelService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>(env);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(CorsPolicyConstants.Default);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}