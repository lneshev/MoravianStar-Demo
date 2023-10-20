using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoravianStar.WebAPI.Middlewares;
using MoravianStar.WebAPI.Transformers;
using MoravianStar_Demo.Maintenance.Services.Services;
using MoravianStar_Demo.Maintenance.WebAPI.Infrastructure.Constants;
using MoravianStar_Demo.Persistence.DbContexts;

namespace MoravianStar_Demo.Maintenance.WebAPI
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
            services
                    .AddControllers(options =>
                    {
                        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                    })
                    .AddControllersAsServices()
                    .AddNewtonsoftJson();

            services.AddSwaggerGen();

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

            services
                .AddDbContextFactory<ClientContext>(options =>
                {
                    options.UseSqlServer(".", sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                }, ServiceLifetime.Singleton);

            services
                .AddDbContextFactory<ClientDMLContext>(options =>
                {
                    options.UseSqlServer(".", sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                }, ServiceLifetime.Singleton);

            services.AddTransient<IDbUpdater, DbUpdater>();
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