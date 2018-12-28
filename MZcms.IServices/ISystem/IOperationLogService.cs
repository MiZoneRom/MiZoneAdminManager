using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MZcms.Model;
using MZcms.DTO.QueryModel;
using MZcms.CommonModel;

namespace MZcms.IServices
{
    public interface IOperationLogService : IService
    {
        /// <summary>
        /// 根据查询条件分页获取操作日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        QueryPageModel<Logs> GetPlatformOperationLogs(OperationLogQuery query);

        /// <summary>
        /// 增加平台日志信息
        /// </summary>
        /// <param name="info"></param>
        void AddPlatformOperationLog(Logs info);

        /// <summary>
        ///根据ID删除平台日志信息
        /// </summary>
        /// <param name="id"></param>
        void DeletePlatformOperationLog(long id);
    }
}
