using Hangfire;
using Hangfire.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Jobs.Server
{
    /// <summary>
    /// Allows working with reccuring jobs.
    /// </summary>
    public static class RecurringJob
    {
        private static readonly Lazy<ConcurrentDictionary<string, bool>> keys = new Lazy<ConcurrentDictionary<string, bool>>(() =>
        {
            return new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        });

        public static void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, Func<string> cronExpression, RecurringJobOptions options = null)
        {
            if (!keys.Value.TryAdd(recurringJobId, true))
            {
                throw new InvalidOperationException($"A job with key: '{recurringJobId}' was already added in the code.");
            }
            Hangfire.RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, options ?? new RecurringJobOptions());
        }

        public static void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, Func<string> cronExpression, RecurringJobOptions options = null)
        {
            if (!keys.Value.TryAdd(recurringJobId, true))
            {
                throw new InvalidOperationException($"A job with key: '{recurringJobId}' was already added in the code.");
            }
            Hangfire.RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, options ?? new RecurringJobOptions());
        }

        public static void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, string cronExpression, RecurringJobOptions options = null)
        {
            if (!keys.Value.TryAdd(recurringJobId, true))
            {
                throw new InvalidOperationException($"A job with key: '{recurringJobId}' was already added in the code.");
            }
            Hangfire.RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, options ?? new RecurringJobOptions());
        }

        public static void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression, RecurringJobOptions options = null)
        {
            if (!keys.Value.TryAdd(recurringJobId, true))
            {
                throw new InvalidOperationException($"A job with key: '{recurringJobId}' was already added in the code.");
            }
            Hangfire.RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, options ?? new RecurringJobOptions());
        }

        public static void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, Func<string> cronExpression, RecurringJobOptions options = null)
        {
            if (!keys.Value.TryAdd(recurringJobId, true))
            {
                throw new InvalidOperationException($"A job with key: '{recurringJobId}' was already added in the code.");
            }
            Hangfire.RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, options ?? new RecurringJobOptions());
        }

        public static void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, Func<string> cronExpression, RecurringJobOptions options = null)
        {
            if (!keys.Value.TryAdd(recurringJobId, true))
            {
                throw new InvalidOperationException($"A job with key: '{recurringJobId}' was already added in the code.");
            }
            Hangfire.RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, options ?? new RecurringJobOptions());
        }

        public static void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression, RecurringJobOptions options = null)
        {
            if (!keys.Value.TryAdd(recurringJobId, true))
            {
                throw new InvalidOperationException($"A job with key: '{recurringJobId}' was already added in the code.");
            }
            Hangfire.RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, options ?? new RecurringJobOptions());
        }

        public static void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression, RecurringJobOptions options = null)
        {
            if (!keys.Value.TryAdd(recurringJobId, true))
            {
                throw new InvalidOperationException($"A job with key: '{recurringJobId}' was already added in the code.");
            }
            Hangfire.RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, options ?? new RecurringJobOptions());
        }

        public static void RemoveIfExists(string recurringJobId)
        {
            keys.Value.TryRemove(recurringJobId, out _);
            Hangfire.RecurringJob.RemoveIfExists(recurringJobId);
        }

        public static void RemoveJobsDeletedFromCode()
        {
            List<string> jobsInDb;

            using (IStorageConnection storageConnection = JobStorage.Current.GetConnection())
            {
                jobsInDb = storageConnection.GetRecurringJobs().Select(x => x.Id).ToList();
            }

            var jobsInCode = keys.Value.Keys.ToList();
            var jobsToRemove = jobsInDb.Except(jobsInCode, StringComparer.OrdinalIgnoreCase);
            foreach (var job in jobsToRemove)
            {
                RemoveIfExists(job);
            }
        }

        public static void TriggerJob(string recurringJobId)
        {
            Hangfire.RecurringJob.TriggerJob(recurringJobId);
        }
    }
}