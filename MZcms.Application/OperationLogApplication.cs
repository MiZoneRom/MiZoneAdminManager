using MZcms.CommonModel;
using MZcms.Core;
using MZcms.IServices;
using MZcms.DTO.QueryModel;
using MZcms.Model;
namespace MZcms.Application
{
    public class OperationLogApplication
    {
        private static IOperationLogService _iOperationLogService = ObjectContainer.Current.Resolve<IOperationLogService>();
        /// <summary>
        /// 根据查询条件分页获取操作日志信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryPageModel<Logs> GetPlatformOperationLogs(OperationLogQuery query)
        {
            return _iOperationLogService.GetPlatformOperationLogs(query);
        }

        /// <summary>
        /// 增加平台日志信息
        /// </summary>
        /// <param name="info"></param>
        public static void AddPlatformOperationLog(Logs info)
        {
             _iOperationLogService.AddPlatformOperationLog(info);
        }

        /// <summary>
        ///根据ID删除平台日志信息
        /// </summary>
        /// <param name="id"></param>
        public static void DeletePlatformOperationLog(long id)
        {
            _iOperationLogService.DeletePlatformOperationLog(id);
        }
    }
}
