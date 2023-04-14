using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Constants.Test;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoravianStar_Demo.Common.Core.Entities.Test
{
    public class BlockEntity : EntityBase<int>
    {
        [MaxLength(BlockEntityConstants.NameMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual ClientEntity Client { get; set; }
        public int? ClientId { get; set; }

        public Polygon Boundaries { get; set; }
    }
}