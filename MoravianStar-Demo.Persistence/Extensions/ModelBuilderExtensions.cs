using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoravianStar.Dao;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MoravianStar_Demo.Persistence.Extensions
{
    /// <summary>
    /// EF Core ModelBuilder extensions
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Creates a many-to-many relationship between two entities.
        /// <para>
        /// Since EF Core 5 it is possible to create many-to-many relationships without explicitly creating a many-to-many entity - EF Core is smart enought to do it by itself.
        /// The problem is that if the many-to-many table and its columns should be named differently from the EF Core's conventions,
        /// a fluent api configuration should be written, which requires writing a lot of code. This extension method simplifies this process.
        /// </para>
        /// </summary>
        /// <typeparam name="TLeftEntity">The type of the left entity.</typeparam>
        /// <typeparam name="TRightEntity">The type of the right entity.</typeparam>
        /// <param name="modelBuilder">The source model builder.</param>
        /// <param name="leftCollection">The collection from the left entity for the relationship.</param>
        /// <param name="rightCollection">The collection from the right entity for the relationship.</param>
        /// <param name="leftColumnName">Left column's name.</param>
        /// <param name="rightColumnName">Right column's name.</param>
        /// <param name="tableName">Many-to-many table's name.</param>
        /// <returns>The builder for the join (many-to-many) entity type.</returns>
        public static EntityTypeBuilder<Dictionary<string, object>> ManyToMany<TLeftEntity, TRightEntity>(
            this ModelBuilder modelBuilder,
            Expression<Func<TLeftEntity, IEnumerable<TRightEntity>>> leftCollection,
            Expression<Func<TRightEntity, IEnumerable<TLeftEntity>>> rightCollection,
            string leftColumnName,
            string rightColumnName,
            string tableName)
            where TLeftEntity : class, IEntityBase
            where TRightEntity : class, IEntityBase
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (leftCollection == null)
            {
                throw new ArgumentNullException(nameof(leftCollection));
            }

            if (rightCollection == null)
            {
                throw new ArgumentNullException(nameof(rightCollection));
            }

            if (string.IsNullOrWhiteSpace(leftColumnName))
            {
                throw new ArgumentNullException(nameof(leftColumnName));
            }

            if (string.IsNullOrWhiteSpace(rightColumnName))
            {
                throw new ArgumentNullException(nameof(rightColumnName));
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(rightColumnName));
            }

            var leftTableName = modelBuilder.Entity<TLeftEntity>().Metadata.GetTableName();
            var rightTableName = modelBuilder.Entity<TRightEntity>().Metadata.GetTableName();

            return modelBuilder.Entity<TLeftEntity>()
                .HasMany(leftCollection)
                .WithMany(rightCollection)
                .UsingEntity<Dictionary<string, object>>(
                    tableName,
                    x => x
                        .HasOne<TRightEntity>(rightTableName)
                        .WithMany()
                        .HasForeignKey(rightColumnName),
                    x => x
                        .HasOne<TLeftEntity>(leftTableName)
                        .WithMany()
                        .HasForeignKey(leftColumnName));
        }
    }
}