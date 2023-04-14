using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Constants.Test;
using System.ComponentModel.DataAnnotations;

namespace MoravianStar_Demo.Web.Core.Models.Test
{
    public class ClientModel : ModelBase<int>
    {
        [Required]
        [MaxLength(ClientEntityConstants.NameMaxLength)]
        public string Name { get; set; }
    }
}