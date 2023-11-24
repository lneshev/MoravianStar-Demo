using MoravianStar.GraphQL.Attributes;
using MoravianStar_Demo.Common.Core.Constants.Test;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoravianStar_Demo.Mobile.Core.GraphQL.Mutations.Test
{
    public class SaveClientInput
    {
        [Required]
        [MaxLength(ClientEntityConstants.NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(ClientEntityConstants.DescriptionMaxLength)]
        [Required]
        public string Description { get; set; }

        //[Required]
        [ValidateChildProperty]
        public SaveAddressInput MainAddress { get; set; }

        [ValidateChildProperty]
        public List<SaveAddressInput> Addresses { get; set; }
    }
}