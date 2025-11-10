using System;

namespace MoravianStar_Demo.Common.DataAccess.Attributes
{
    /// <summary>
    /// This attribute should be applied on <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{TEntity}"/> intances.
    /// The attribute makes the relationship between the entity configuration and a DbContext. The parameter <see cref="IsSynonymInTheOtherContext"/>
    /// specifies if the entity should be included in the other DbContext as a synonym.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ForDbContextAttribute : Attribute
    {
        public Type DbContextType { get; private set; }
        public bool IsSynonymInTheOtherContext { get; private set; }

        public ForDbContextAttribute(Type dbContextType, bool isSynonymInTheOtherContext = false)
        {
            DbContextType = dbContextType;
            IsSynonymInTheOtherContext = isSynonymInTheOtherContext;
        }
    }
}