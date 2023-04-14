using MoravianStar.Dao;
using System.ComponentModel.DataAnnotations;

namespace MoravianStar_Demo.Common.Core.Entities.Test
{
    public class LanguageEntity : EntityBase<int>
    {
        [Required]
        public string Name { get; set; }
    }
}