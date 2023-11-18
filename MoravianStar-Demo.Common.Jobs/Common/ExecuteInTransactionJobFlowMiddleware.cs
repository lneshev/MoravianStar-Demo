using Microsoft.Extensions.DependencyInjection;
using MoravianStar.Dao;
using MoravianStar.DependencyInjection;
using System;
using System.Threading.Tasks;
using MS = MoravianStar.Dao;

namespace MoravianStar_Demo.Common.Jobs.Common
{
    public class ExecuteInTransactionJobFlowMiddleware : IJobFlowMiddleware
    {
        public ExecuteInTransactionJobFlowMiddleware()
        {
            DbContextType = MS.Persistence.DefaultDbContextType;
        }

        public ExecuteInTransactionJobFlowMiddleware(Type dbContextType)
        {
            DbContextType = dbContextType;
        }

        public Type DbContextType { get; }

        public async Task InvokeAsync(Func<Task> next)
        {
            var serviceType = typeof(IDbTransaction<>).MakeGenericType(DbContextType);
            var dbTransaction = (IDbTransaction)ServiceLocator.Container.GetRequiredService(serviceType);
            await dbTransaction.BeginAsync();

            try
            {
                await next();
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }

            await dbTransaction.CommitAsync();
        }
    }
}