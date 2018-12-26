using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MZcms.Core
{
	public static class LinqHelper
	{
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> tKeys = new HashSet<TKey>();
			foreach (TSource tSource in source)
			{
				if (tKeys.Add(keySelector(tSource)))
				{
					yield return tSource;
				}
			}
		}

		public static Expression<Func<T, bool>> GetDefaultPredicate<T>(this IQueryable<T> source, bool defultPredicate)
		{
			Expression<Func<T, bool>> expression;
			expression = (!defultPredicate ? PredicateExtensions.False<T>() : PredicateExtensions.True<T>());
			return expression;
		}

		public static Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderBy<T>(this IQueryable<T> source, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
		{
			if (orderBy == null)
			{
				throw new ArgumentNullException("初始排序不可为空");
			}
			return orderBy;
		}
	}
}