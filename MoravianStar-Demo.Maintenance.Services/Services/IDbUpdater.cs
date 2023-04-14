using MoravianStar_Demo.Maintenance.Core.DTOs;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Maintenance.Services.Services
{
    public interface IDbUpdater
    {
        Task<DbsUpdateResult> CreateAndUpdateAllAsync();
    }
}