using MoravianStar.Dao;
using MoravianStar_Demo.Persistence.DbContexts;

namespace MoravianStar_Demo.Web.WebAPI.Controllers
{
    /// <inheritdoc cref="EntityRestController{TEntity, TId, TModel, TFilter, ClientDMLContext}"/>
    public abstract class ClientEntityRestController<TEntity, TId, TModel, TFilter> : EntityRestController<TEntity, TId, TModel, TFilter, ClientDMLContext>
        where TEntity : class, IEntityBase<TId>, IProjectionBase, new()
        where TModel : class, IModelBase<TId>, IProjectionBase, new()
        where TFilter : FilterSorterBase<TEntity>, new()
    {
    }
}