using Hangfire.Common;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using MoravianStar.Dao;
using MoravianStar.DependencyInjection;
using System;

namespace MoravianStar_Demo.Common.Jobs.Common
{
    /// <summary>
    /// An <see cref="IServerFilter"/> attribute that wraps the flow in a database transaction.
    /// </summary>
    [Obsolete("The current Hangfire version doesn't suport async job execution.")]
    public class ExecuteInTransactionAttribute : JobFilterAttribute, IServerFilter
    {
        private IDbTransaction dbTransaction;

        public ExecuteInTransactionAttribute()
        {
            DbContextType = Persistence.DefaultDbContextType;
        }

        public ExecuteInTransactionAttribute(Type dbContextType)
        {
            DbContextType = dbContextType;
        }

        public Type DbContextType { get; }

        public void OnPerforming(PerformingContext context)
        {
            var serviceType = typeof(IDbTransaction<>).MakeGenericType(DbContextType);
            dbTransaction = (IDbTransaction)ServiceLocator.Container.GetRequiredService(serviceType);
            dbTransaction.Begin();
        }

        public void OnPerformed(PerformedContext context)
        {
            if (context.Exception == null)
            {
                dbTransaction.Commit();
            }
            else
            {
                dbTransaction.Rollback();
            }
        }
    }
}