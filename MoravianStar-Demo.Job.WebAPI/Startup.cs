using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoravianStar.Dao;
using MoravianStar.Settings;
using MoravianStar.WebAPI.Extensions;
using MoravianStar.WebAPI.Swagger;
using MoravianStar_Demo.Common.Core.DTOs.Test;
using MoravianStar_Demo.Common.Core.Enums.Test;
using MoravianStar_Demo.Common.Core.Resources;
using MoravianStar_Demo.Common.Jobs.Client.SqlServer;
using MoravianStar_Demo.Common.Jobs.Jobs;
using MoravianStar_Demo.Job.WebAPI.Infrastructure.Constants;
using MoravianStar_Demo.Persistence.DbContexts;
using JobsClient = MoravianStar_Demo.Common.Jobs.Client;

namespace MoravianStar_Demo.Job.WebAPI
{
    /// <summary>
    /// WARNING! The current Hangfire version does NOT support async jobs, so if a job or a filter is trying to execute an async code, it will fail in most of the times!
    /// For this reason, I left filters "ServiceLocatorAttribute" and "ExecuteInTransactionAttribute" commented in the code and created a new custom "flow" that allows executing of
    /// async code. See: "IJobFlow", "JobFlowBase" and "IJobFlowMiddleware". I hope the async execution to become available in Hangfire 2.0.0 as promised by the author.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfireWithSqlServerStorage(Configuration.GetConnectionString("Hangfire"), new SqlServerStorageOptions() { PrepareSchemaIfNecessary = true }, (sp, gc) =>
            {
                //gc.UseFilter(new ServiceLocatorAttribute(sp)); // Commented, because the current Hangfire version doesn't suport async job execution
            });
            services.AddHangfireServer(x => { x.WorkerCount = 1; });

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.DocumentFilter<HideInDocsFilter>();
            });

            services
                .AddDbContextPool<SystemContext>(options =>
                {
                    options.UseSqlServer(Configuration["ConnectionStrings:System"], sqlServerOptions =>
                    {
                        sqlServerOptions.UseNetTopologySuite();
                    });
                });

            services.AddScoped<IDbTransaction<SystemContext>, DbTransaction<SystemContext>>();

            services.AddTransient<IExampleJobFlow, ExampleJobFlow>();
            services.AddTransient<IExampleJobProcessor, ExampleJobProcessor>();

            services.AddTransient<IExampleJob2Processor, ExampleJob2Processor>();
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

            //Dashboard
            app.UseHangfireDashboard("/hangfire", new DashboardOptions() { AppPath = "/swagger" });

            //Client
            RegisterCronJobs();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(CorsPolicyConstants.Default);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterCronJobs()
        {
            //var s1 = BackgroundJob.Schedule(() => Console.WriteLine("First task!"), TimeSpan.FromSeconds(20));
            //BackgroundJob.Enqueue(() => kvsService.Import(currentUserId));
            //BackgroundJob.Schedule(() => Console.WriteLine("Hello world from Hangfire 2!"), TimeSpan.FromSeconds(20));

            //var s2 = BackgroundJob.ContinueJobWith(s1, () => Console.WriteLine("Second task!"));
            //BackgroundJob.ContinueJobWith<IExampleService>(s2, x => x.ExecuteAsync("gg", CancellationToken.None));

            JobsClient.RecurringJob.AddOrUpdate<IExampleJobFlow>(JobIds.First, x => x.Process(new ExampleJobSenderMessage() { ClientId = 1 }), Cron.Minutely);
            //JobsClient.RecurringJob.AddOrUpdate(JobIds.Last, () => Console.WriteLine("Last"), Cron.Minutely);

            JobsClient.RecurringJob.RemoveJobsDeletedFromCode();
        }
    }
}