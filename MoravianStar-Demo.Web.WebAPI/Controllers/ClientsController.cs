using Microsoft.AspNetCore.Mvc;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Persistence.DbContexts;
using MoravianStar_Demo.Web.Core.Models.Test;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.WebAPI.Controllers
{
    public class ClientsController : SystemEntityRestController<ClientEntity, int, ClientModel, ClientFilter>
    {
        private readonly IEntityRepository<ClientEntity, DataLayer_SystemContext> clientER;

        public ClientsController()
        {
            clientER = MoravianStar.Dao.Persistence.ForDbContext<DataLayer_SystemContext>().ForEntity<ClientEntity>();
        }

        public override async Task<ActionResult<ClientModel>> Get([FromRoute] int id)
        {
            return await base.Get(id);
        }

        public override async Task<ActionResult<PageResult<ClientModel>>> Read([FromQuery] ClientFilter filter, [FromQuery] List<Sort> sorts, [FromQuery] Page page)
        {
            //var model1 = await modelRepository.MapAsync(await repository.ReadQuery(filter, sorts, null, modelService.Project(), false).FirstOrDefaultAsync());
            //var model2 = await modelRepository.MapAsync(await repository.ReadQuery(filter, null, null, modelService.Project(), false).SingleOrDefaultAsync());
            //var model3 = await clientER.ReadQuery<ClientEntity, ClientFilter>(filter, sorts, page, x => x.Include(y => y.MainAddress).DefaultIfEmpty()).ToListAsync();

            //var clientA = (await clientER.ReadAsync<ClientFilter>(
            //    new ClientFilter() { NameEquals = "Client A" },
            //    includes: x => x.Include(y => y.MainAddress))
            //).Single();

            //var clientAMainAddress = clientA.MainAddress.Address;

            return await base.Read(filter, sorts, page);
        }

        public override async Task<ActionResult<int>> Count([FromQuery] ClientFilter filter)
        {
            return await base.Count(filter);
        }

        public override async Task<ActionResult<bool>> Exist([FromQuery] ClientFilter filter)
        {
            return await base.Exist(filter);
        }

        public override async Task<ActionResult<ClientModel>> Post([FromBody] ClientModel model)
        {
            return await base.Post(model);
        }

        public override async Task<ActionResult<ClientModel>> Put([FromRoute] int id, [FromBody] ClientModel model)
        {
            return await base.Put(id, model);
        }

        public override async Task<ActionResult<ClientModel>> Delete([FromRoute] int id)
        {
            return await base.Delete(id);
        }
    }
}