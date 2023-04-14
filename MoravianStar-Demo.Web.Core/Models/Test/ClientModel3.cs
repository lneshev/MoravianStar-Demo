using MoravianStar.Dao;
using System;
using System.Collections.Generic;

namespace MoravianStar_Demo.Web.Core.Models.Test
{
    public class ClientModel3 : ModelBase<int>
    {
        public ClientModel3()
        {
            VehiclesLicensePlates = new List<string>();
        }

        public string Name { get; set; }
        public Guid? MainAddressId { get; set; }
        public string MainAddressAddress { get; set; }
        public int AddressesCount { get; set; }
        public List<string> VehiclesLicensePlates { get; set; }
    }
}