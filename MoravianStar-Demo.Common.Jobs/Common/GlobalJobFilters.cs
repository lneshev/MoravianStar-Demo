using Hangfire.Common;

namespace MoravianStar_Demo.Common.Jobs.Common
{
    /// <inheritdoc cref="Hangfire.GlobalJobFilters" />
    public static class GlobalJobFilters
    {
        public static JobFilterCollection Filters
        {
            get { return Hangfire.GlobalJobFilters.Filters; }
        }
    }
}