using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Constants.Test;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoravianStar_Demo.Common.Core.Entities.Test
{
    public class AddressEntity : EntityBase<Guid>
    {
        [Required]
        [MaxLength(AddressEntityConstants.AddressMaxLength)]
        public string Address { get; set; }
    }
}