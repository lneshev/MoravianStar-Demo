using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoravianStar.WebAPI.Swagger;
using MoravianStar_Demo.Common.Jobs.Client;
using MoravianStar_Demo.Common.Jobs.Client.SqlServer;
using MoravianStar_Demo.Common.Jobs.Common;
using MoravianStar_Demo.Common.Jobs.Dashboard;
using MoravianStar_Demo.Common.Jobs.Server;
using MoravianStar_Demo.Job.WebAPI.Infrastructure.Constants;
using System;

namespace MoravianStar_Demo.Job.WebAPI
{
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
            services.AddHangfireWithSqlServerStorage(Configuration.GetConnectionString("Hangfire"), new SqlServerStorageOptions() { PrepareSchemaIfNecessary = true });
            services.AddHangfireServer();
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.DocumentFilter<HideInDocsFilter>();
            });
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
            var s1 = BackgroundJob.Schedule(() => Console.WriteLine("First task!"), TimeSpan.FromSeconds(20));
            //BackgroundJob.Enqueue(() => kvsService.Import(currentUserId));
            BackgroundJob.Schedule(() => Console.WriteLine("Hello world from Hangfire 2!"), TimeSpan.FromSeconds(20));

            var s2 = BackgroundJob.ContinueJobWith(s1, () => Console.WriteLine("Second task!"));
            //BackgroundJob.ContinueJobWith<IExampleService>(s2, x => x.ExecuteAsync("gg", CancellationToken.None));

            RecurringJob.AddOrUpdate(JobIds.First, () => Console.WriteLine("First"), Hangfire.Cron.Minutely);
            RecurringJob.AddOrUpdate(JobIds.Last, () => Console.WriteLine("Last"), Hangfire.Cron.Minutely);

            RecurringJob.RemoveJobsDeletedFromCode();
        }
    }
}