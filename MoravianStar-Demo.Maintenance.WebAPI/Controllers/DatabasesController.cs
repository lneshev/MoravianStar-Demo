using Microsoft.AspNetCore.Mvc;
using MoravianStar_Demo.Maintenance.Core.DTOs;
using MoravianStar_Demo.Maintenance.Services.Services;
using MoravianStar_Demo.Maintenance.WebAPI.Infrastructure.Constants;
using System;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Maintenance.WebAPI.Controllers
{
    [ApiController]
    [Route(RoutingConstants.ApiController)]
    public class DatabasesController : ControllerBase
    {
        private readonly IDbUpdater dbUpdater;

        public DatabasesController(IDbUpdater dbUpdater)
        {
            this.dbUpdater = dbUpdater;
        }

        [HttpPost(RoutingConstants.Action)]
        public async Task<ActionResult<DbsUpdateResult>> CreateAndUpdateAllDatabases()
        {
            return await dbUpdater.CreateAndUpdateAllAsync();
        }

        [HttpPost(RoutingConstants.Action)]
        public async Task<ActionResult<DbUpdateResult>> CreateNewClientDatabase()
        {
            throw new NotImplementedException();
        }
    }
}