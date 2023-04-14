using DataLayer.Common.Core.Entities.Test;
using DataLayer.Common.Core.Interfaces;
using DataLayer.Common.Core.Projections;
using DataLayer.Common.Services;
using DataLayer.Web.Core.Models.Test;
using DataLayer.Persistence.DbContexts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class ClientModelService : SystemModelService<ClientEntity, int, ClientModel>
    {
        private readonly IRepository<DataLayer_SystemContext> repository;

        public ClientModelService(IRepository<DataLayer_SystemContext> repository) : base(repository)
        {
            this.repository = repository;
        }

        public override async Task<ClientModel> CreateAsync(ClientModel model)
        {
            using (var tx = await repository.BeginTransactionAsync())
            {
                var entity = new ClientEntity();

                entity.Name = model.Name;

                entity = await repository.AddAsync(entity);
                await repository.SaveChangesAsync();
                await tx.CommitAsync();

                model = await MapAsync(entity);
            }

            return model;
        }

        public override async Task<ClientModel> UpdateAsync(ClientModel model)
        {
            using (var tx = await repository.BeginTransactionAsync())
            {
                var entity = await repository.GetAsync<ClientEntity, int>(model.Id);

                entity.Name = model.Name;

                entity = repository.Update(entity);
                await repository.SaveChangesAsync();
                await tx.CommitAsync();

                model = await MapAsync(entity);
            }

            return model;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using (var tx = await repository.BeginTransactionAsync())
            {
                var entity = await repository.GetAsync<ClientEntity, int>(id);
                if (entity.MainAddressId.HasValue)
                {
                    await repository.DeleteAsync<AddressEntity, Guid>(entity.MainAddressId.Value);
                }
                repository.Delete(entity);
                await repository.SaveChangesAsync();
                await tx.CommitAsync();
            }

            return true;
        }

        public override Expression<Func<ClientEntity, IProjectionBase>> Project()
        {
            return entity => new ClientModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}