using Microsoft.AspNetCore.Mvc;
using MoravianStar.WebAPI.Constants;
using MoravianStar_Demo.Common.Jobs.Jobs;

namespace MoravianStar_Demo.Job.WebAPI.Controllers
{
    [ApiController]
    [Route(RoutingConstants.ApiController)]
    public class ExampleJobs2Controller : ControllerBase
    {
        [HttpPost]
        public void Post()
        {
            Jobs.ExampleJob2();
        }
    }
}