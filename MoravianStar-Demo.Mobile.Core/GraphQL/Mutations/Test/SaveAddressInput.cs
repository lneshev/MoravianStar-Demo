using MoravianStar_Demo.Common.Core.Constants.Test;
using System.ComponentModel.DataAnnotations;

namespace MoravianStar_Demo.Mobile.Core.GraphQL.Mutations.Test
{
    public class SaveAddressInput
    {
        [Required]
        [MaxLength(AddressEntityConstants.AddressMaxLength)]
        [MinLength(2)]
        public string Address { get; set; }
    }
}