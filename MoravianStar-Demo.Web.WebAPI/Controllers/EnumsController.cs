using Microsoft.AspNetCore.Mvc;
using MoravianStar.Extensions;
using System.Collections.Generic;

namespace MoravianStar_Demo.Web.WebAPI.Controllers
{
    public class EnumsController : MoravianStar.WebAPI.Controllers.EnumsController
    {
        public override ActionResult<List<EnumNameValue>> Get()
        {
            return base.Get();
        }

        public override ActionResult<List<EnumTextValue>> Get([FromRoute] string enumName, [FromQuery] List<int> exactEnumValues, [FromQuery] bool sortByText = false)
        {
            return base.Get(enumName, exactEnumValues, sortByText);
        }
    }
}