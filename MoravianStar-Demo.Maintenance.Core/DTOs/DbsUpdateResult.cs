using MoravianStar_Demo.Maintenance.Core.Enums;
using System.Collections.Generic;

namespace MoravianStar_Demo.Maintenance.Core.DTOs
{
    public class DbsUpdateResult
    {
        public DbsUpdateResult()
        {
            Results = new List<DbUpdateResult>();
        }

        public DbsUpdateState State { get; set; }
        public List<DbUpdateResult> Results { get; set; }
    }
}