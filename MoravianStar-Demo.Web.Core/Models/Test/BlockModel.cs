using MoravianStar.Dao;
using NetTopologySuite.Geometries;

namespace MoravianStar_Demo.Web.Core.Models.Test
{
    public class BlockModel : ModelBase<int>
    {
        public string ClientName { get; set; }
        public Polygon Boundaries { get; set; }
        public double? BoundariesArea { get; set; }
    }
}