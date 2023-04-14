using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoravianStar.Dao;
using MoravianStar_Demo.Web.WebAPI.Infrastructure.Constants;

namespace MoravianStar_Demo.Web.WebAPI.Controllers
{
    /// <summary>
    /// The base WebAPI controller for the most common operations over an entity (like CRUD, count, exist, etc.), defined in the REST standard.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the Id of the entity.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    [Route(RoutingConstants.ApiController)]
    public abstract class EntityRestController<TEntity, TId, TModel, TFilter, TDbContext> : MoravianStar.WebAPI.Controllers.EntityRestController<TEntity, TId, TModel, TFilter, TDbContext>
        where TEntity : class, IEntityBase<TId>, IProjectionBase, new()
        where TModel : class, IModelBase<TId>, IProjectionBase, new()
        where TFilter : FilterSorterBase<TEntity>, new()
        where TDbContext : DbContext
    {
    }
}