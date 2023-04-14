using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Constants.Test;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoravianStar_Demo.Common.Core.Entities.Test
{
    public class VehicleEntity : EntityBase<int>
    {
        [Required]
        [MaxLength(VehicleEntityConstants.LicensePlateMaxLength)]
        public string LicensePlate { get; set; }

        public Point CurrentLocation { get; set; }

        public List<ClientEntity> Clients { get; set; }
    }
}