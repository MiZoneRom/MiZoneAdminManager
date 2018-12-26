using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MZcms.Core
{
	public static class PredicateExtensions
	{
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			Expression<Func<T, bool>> expression = first.Compose<Func<T, bool>>(second, new Func<Expression, Expression, Expression>(Expression.And));
			return expression;
		}

		public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
		{
			Dictionary<ParameterExpression, ParameterExpression> dictionary = first.Parameters.Select((ParameterExpression f, int i) => new { f = f, s = second.Parameters[i] }).ToDictionary((p) => p.s, (p) => p.f);
			Expression expression = ParameterRebinder.ReplaceParameters(dictionary, second.Body);
			Expression<T> expression1 = Expression.Lambda<T>(merge(first.Body, expression), first.Parameters);
			return expression1;
		}

		public static Expression<Func<T, bool>> False<T>()
		{
			return (T f) => false;
		}

		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			Expression<Func<T, bool>> expression = first.Compose<Func<T, bool>>(second, new Func<Expression, Expression, Expression>(Expression.Or));
			return expression;
		}

		public static Expression<Func<T, bool>> True<T>()
		{
			return (T f) => true;
		}
	}
}