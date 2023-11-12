using Microsoft.Extensions.Options;
using MoravianStar_Demo.Common.Core.Configuration;
using MoravianStar_Demo.Common.Core.DTOs.Test;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Services.Test
{
    public class ExampleJobSender : IExampleJobSender
    {
        private readonly ApplicationsConfiguration applicationsConfiguration;

        public ExampleJobSender(IOptions<ApplicationsConfiguration> applicationsConfiguration)
        {
            this.applicationsConfiguration = applicationsConfiguration.Value ?? throw new ArgumentNullException(nameof(ApplicationsConfiguration));
        }

        public async Task SendJob(ExampleJobSenderMessage message)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(applicationsConfiguration.JobWebAPI.ApplicationUrl)
            };

            var httpResponseMessage = await httpClient.PostAsJsonAsync("api/ExampleJobs", message);
        }
    }
}