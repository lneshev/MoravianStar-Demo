using MoravianStar.Dao;
using System;

namespace MoravianStar_Demo.Web.Core.Models.Test
{
    public class AddressModel : ModelBase<Guid>
    {
        public string Address { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
    }
}