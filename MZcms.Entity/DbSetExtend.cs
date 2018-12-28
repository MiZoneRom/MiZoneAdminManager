using MZcms.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MZcms.Entity
{
	public static class DbSetExtend
	{
		public static bool Exist<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where)
		where TEntity : BaseModel
		{
			return dbSet.Count(where) > 0;
		}

		public static IQueryable<TEntity> FindAll<TEntity>(this DbSet<TEntity> dbSet)
		where TEntity : BaseModel
		{
			return 
				from item in dbSet
				where true
				select item;
		}

		public static IQueryable<TEntity> FindAll<TEntity, TKey>(this DbSet<TEntity> dbSet, int pageNumber, int pageSize, out int total, Expression<Func<TEntity, TKey>> orderBy, bool isAsc = true)
		where TEntity : BaseModel
		{
			total = dbSet.Count();
			IQueryable<TEntity> tEntities = 
				from item in dbSet
				where true
				select item;
			tEntities = (!isAsc ? tEntities.OrderByDescending<TEntity, TKey>(orderBy) : tEntities.OrderBy<TEntity, TKey>(orderBy));
			tEntities = tEntities.Skip((pageNumber - 1) * pageSize).Take(pageSize);
			return tEntities;
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where)
		where TEntity : BaseModel
		{
			return dbSet.Where(where);
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this IQueryable<TEntity> entities, Expression<Func<TEntity, bool>> where)
		where TEntity : BaseModel
		{
			return entities.Where(where);
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where, int pageNumber, int pageSize, out int total)
		where TEntity : BaseModel
		{
			total = dbSet.Count(where);
			return dbSet.Where(where).OrderBy<TEntity, object>((TEntity item) => item.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize);
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this IQueryable<TEntity> entities, Expression<Func<TEntity, bool>> where, int pageNumber, int pageSize, out int total)
		where TEntity : BaseModel
		{
			total = entities.Count(where);
			return entities.Where(where).OrderBy<TEntity, object>((TEntity item) => item.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize);
		}

		public static IQueryable<TEntity> FindBy<TEntity, TKey>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where, int pageNumber, int pageSize, out int total, Expression<Func<TEntity, TKey>> orderBy, bool isAsc = true)
		where TEntity : BaseModel
		{
			IQueryable<TEntity> tEntities;
			total = dbSet.Count(where);
			tEntities = (!isAsc ? dbSet.Where(where).OrderByDescending<TEntity, TKey>(orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize) : dbSet.Where(where).OrderBy<TEntity, TKey>(orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize));
			return tEntities;
		}

		public static IQueryable<TEntity> FindBy<TEntity, TKey>(this IQueryable<TEntity> entities, Expression<Func<TEntity, bool>> where, int pageNumber, int pageSize, out int total, Expression<Func<TEntity, TKey>> orderBy, bool isAsc = true)
		{
			IQueryable<TEntity> tEntities;
			total = entities.Count(where);
			tEntities = (!isAsc ? entities.Where(where).OrderByDescending<TEntity, TKey>(orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize) : entities.Where(where).OrderBy<TEntity, TKey>(orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize));
			return tEntities;
		}

		public static TEntity FindById<TEntity>(this DbSet<TEntity> dbSet, object id)
		where TEntity : BaseModel
		{
			return dbSet.FirstOrDefault((TEntity p) => p.Id == id);
		}

		public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> entities, out int total, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int pageNumber = 1, int pageSize = 20)
		{
			if (orderBy == null)
			{
				throw new ArgumentNullException("排序条件不能为空");
			}
			total = entities.Count();
			entities = orderBy(entities);
			IQueryable<TEntity> tEntities = entities.Skip((pageNumber - 1) * pageSize).Take(pageSize);
			return tEntities;
		}

		public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> entities, out int total, int pageNumber = 1, int pageSize = 20, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
		where TEntity : BaseModel
		{
			if (orderBy == null)
			{
				orderBy = (IQueryable<TEntity> d) => 
					from o in d
					orderby o.Id descending
					select o;
			}
			total = entities.Count();
			entities = orderBy(entities);
			IQueryable<TEntity> tEntities = entities.Skip((pageNumber - 1) * pageSize).Take(pageSize);
			return tEntities;
		}

		public static void Remove<TEntity>(this DbSet<TEntity> dbSet, params object[] ids)
		where TEntity : BaseModel
		{
			List<TEntity> tEntities = new List<TEntity>();
			object[] objArray = ids;
			for (int i = 0; i < objArray.Length; i++)
			{
				tEntities.Add(dbSet.FindById<TEntity>(objArray[i]));
			}
			dbSet.RemoveRange(tEntities);
		}

		public static void Remove<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where)
		where TEntity : BaseModel
		{
			dbSet.RemoveRange(dbSet.FindBy(where));
		}
	}
}