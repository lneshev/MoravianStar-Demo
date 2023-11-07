using Hangfire.Annotations;
using Hangfire.Common;
using System;

namespace MoravianStar_Demo.Common.Jobs.Client
{
    /// <inheritdoc cref="Hangfire.BackgroundJob"/>
    public class BackgroundJob : Hangfire.BackgroundJob
    {
        public BackgroundJob([NotNull] string id, [CanBeNull] Job job, DateTime createdAt) : base(id, job, createdAt)
        {
        }
    }
}