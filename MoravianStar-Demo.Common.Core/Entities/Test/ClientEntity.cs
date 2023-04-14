using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Constants.Test;
using MoravianStar_Demo.Common.Core.Enums.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoravianStar_Demo.Common.Core.Entities.Test
{
    public class ClientEntity : EntityBase<int>
    {
        public ClientEntity()
        {
            Addresses = new List<AddressEntity>();
            Vehicles = new List<VehicleEntity>();
        }

        [Required]
        [MaxLength(ClientEntityConstants.NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(ClientEntityConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public ClientStatus Status { get; set; }

        [ForeignKey(nameof(MainAddressId))]
        public AddressEntity MainAddress { get; set; }
        public Guid? MainAddressId { get; set; }

        public List<AddressEntity> Addresses { get; set; }

        public List<VehicleEntity> Vehicles { get; set; }
    }
}