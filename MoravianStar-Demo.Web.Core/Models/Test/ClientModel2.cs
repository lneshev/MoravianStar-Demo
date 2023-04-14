using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Enums.Test;

namespace MoravianStar_Demo.Web.Core.Models.Test
{
    public class ClientModel2 : ModelBase<int>
    {
        public string Name { get; set; }
        public ClientStatus Status { get; set; }
        public string StatusText { get; set; }
    }
}