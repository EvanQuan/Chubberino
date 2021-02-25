using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Chubberino.UnitTests.Utility
{
    public static class IEnumerableExtensions
    {
        public static DbSet<T> ToDbSet<T>(this IList<T> source)
            where T : class
        {
            var dbSet = new Mock<DbSet<T>>();

            var queryable = source.AsQueryable();

            dbSet.As<IQueryable<T>>().SetReturnsDefault(queryable);
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            dbSet.As<IEnumerable<T>>().Setup(m => m.GetEnumerator()).Returns(source.GetEnumerator());
            dbSet.As<IEnumerable>().Setup(m => m.GetEnumerator()).Returns(source.GetEnumerator());
            dbSet.As<IListSource>().Setup(m => m.GetList()).Returns((IList)source);

            dbSet.Setup(x => x.AsQueryable()).Returns(queryable);

            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(s => source.Add(s));

            return dbSet.Object;
        }
    }
}
