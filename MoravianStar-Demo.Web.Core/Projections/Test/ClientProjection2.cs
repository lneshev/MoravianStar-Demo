using DataLayer.Common.Core.Projections;
using MoravianStar_Demo.Common.Core.Enums.Test;

namespace MoravianStar_Demo.Web.Core.Projections.Test
{
    public class ClientProjection2 : ProjectionBase<int>
    {
        public string Name { get; set; }
        public ClientStatus Status { get; set; }
    }
}