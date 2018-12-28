using MZcms.Model;
using System;
using System.Linq.Expressions;

namespace MZcms.DTO.QueryModel
{
    public partial class QueryBase
    {
        public QueryBase()
        {
            PageNo = 1;
            PageSize = 10;
        }

        /// <summary>
        /// 页号
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序属性
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAsc { get; set; }

    }


    public class QueryBase<T, Tout> where T : BaseModel
    {

        /// <summary>
        /// 页号
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序属性
        /// </summary>
        public Expression<Func<T, Tout>> Sort { get; set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAsc { get; set; }

    }
}
