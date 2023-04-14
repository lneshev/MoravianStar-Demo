using MoravianStar_Demo.Maintenance.Core.Enums;
using System;

namespace MoravianStar_Demo.Maintenance.Core.DTOs
{
    public class DbUpdateResult
    {
        public string Name { get; set; }
        public DbUpdateState State { get; set; }
        public Exception Exception { get; set; }
    }
}