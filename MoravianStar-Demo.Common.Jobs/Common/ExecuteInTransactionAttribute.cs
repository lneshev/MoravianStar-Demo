using Hangfire.Common;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using MoravianStar.Dao;
using MoravianStar.DependencyInjection;
using System;
using MS = MoravianStar.Dao;

namespace MoravianStar_Demo.Common.Jobs.Common
{
    /// <summary>
    /// An <see cref="IServerFilter"/> attribute that wraps the flow in a database transaction.
    /// </summary>
    public class ExecuteInTransactionAttribute : JobFilterAttribute, IServerFilter
    {
        public Type DbContextType { get; }
        private IDbTransaction dbTransaction;

        public ExecuteInTransactionAttribute()
        {
            DbContextType = MS.Persistence.DefaultDbContextType;
        }

        public ExecuteInTransactionAttribute(Type dbContextType)
        {
            DbContextType = dbContextType;
        }

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