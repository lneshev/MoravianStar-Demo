using Microsoft.AspNetCore.Mvc;
using MoravianStar.WebAPI.Constants;
using MoravianStar_Demo.Common.Core.DTOs.Test;
using MoravianStar_Demo.Common.Jobs.Client;
using MoravianStar_Demo.Common.Jobs.Jobs;

namespace MoravianStar_Demo.Job.WebAPI.Controllers
{
    [ApiController]
    [Route(RoutingConstants.ApiController)]
    public class ExampleJobsController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] ExampleJobSenderMessage message)
        {
            BackgroundJob.Enqueue<IExampleJobProcessor>(x => x.Process(message));
        }
    }
}